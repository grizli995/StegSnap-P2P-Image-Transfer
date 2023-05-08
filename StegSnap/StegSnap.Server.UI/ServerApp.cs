using Microsoft.Extensions.DependencyInjection;
using StegSharp.Application.Common.Interfaces;
using StegSharp.Infrastructure;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using StegSnap.Domain.Common.Enums;
using StegSnap.Server.Common.Models;
using Message = StegSnap.Server.Common.Models.Message;
using System.Windows.Forms;
using System;

namespace StegSnap.Server.UI
{
    public class ServerApp
    {
        private TcpListener _listener;
        private readonly ConcurrentDictionary<Guid, TcpClient> _clients = new ConcurrentDictionary<Guid, TcpClient>();
        private System.Threading.Timer _imageRequestTimer;
        private int imageRequestInterval = 10000; // 10 seconds
        private readonly Action<Guid, Image, string, string> _onImageReceivedAndDataExtracted;
        private readonly Action<string> _onConsoleWriteLine;

        public int? SnapshotRequestIntervalSeconds;
        public string StoragePath;

        public ServerApp(Action<Guid, Image, string, string> onImageReceivedAndDataExtracted, 
            Action<string> onConsoleWriteLine)
        {
            _onImageReceivedAndDataExtracted = onImageReceivedAndDataExtracted;
            _onConsoleWriteLine = onConsoleWriteLine;

            int port = 3000;
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            AcceptClientsAsync();
        }

        private async void AcceptClientsAsync()
        {
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            var clientId = Guid.NewGuid();
            _clients.TryAdd(clientId, client);

            try
            {
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream);
                using var writer = new StreamWriter(stream);

                await writer.WriteLineAsync(clientId.ToString());
                await writer.FlushAsync();

                SetupSnapshotRequests(writer);

                while (client.Connected)
                {
                    var messageJson = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(messageJson)) break;

                    var message = JsonSerializer.Deserialize<Message>(messageJson);
                    if (message == null) continue;

                    _onConsoleWriteLine?.Invoke($"Received message from {clientId}: {message.Type}");

                    switch (message.Type)
                    {
                        case MessageType.UpdateIPAddress:
                            HandleUpdateIPAddress(clientId, message.Payload);
                            break;
                        case MessageType.SnapshotResponse:
                            HandleReceivedImage(clientId, message.ImageData, message);
                            break;
                        // Add other message types and handling logic here
                        default:
                            _onConsoleWriteLine?.Invoke($"Unknown message type: {message.Type}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _onConsoleWriteLine?.Invoke($"Error handling client: {ex.Message}");
            }
            finally
            {
                _clients.TryRemove(clientId, out _);
                client.Close();
            }
        }

        private void SetupSnapshotRequests(StreamWriter writer)
        {
            var interval = SnapshotRequestIntervalSeconds * 1000 ?? imageRequestInterval;
            _imageRequestTimer = new System.Threading.Timer(async _ =>
            {
                foreach (var client in _clients)
                {
                    await RequestImageFromClientAsync(writer);
                }
            }, null, interval, interval);
        }

        private static void HandleUpdateIPAddress(Guid clientId, string payload)
        {
            // Update client's IP address in _clients, and notify other clients about the update
            // This is just a placeholder method; you need to implement the actual logic
        }


        private static async Task RequestImageFromClientAsync(StreamWriter writer)
        {
            var message = new Message
            {
                Type = MessageType.SnapshotRequest
            };

            var messageJson = JsonSerializer.Serialize(message);
            await writer.WriteLineAsync(messageJson);
            await writer.FlushAsync();
        }


        private static string ExtractMessage(string password, string filePathExtract, IF5Service service)
        {
            using (FileStream fileStream = new FileStream(filePathExtract, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    return service.Extract(password, binaryReader);
                }
            }
        }

        private void HandleReceivedImage(Guid clientId, byte[] imageData, Message message)
        {
            // Process the received image data, e.g., save it to a file
            var imageGuid = Guid.NewGuid();
            var storagePath = StoragePath ?? $"C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\ServerOutput";
            string imageName = $"\\received_image_{imageGuid}_from_{clientId}.jpg";
            string imagePath = $"{storagePath}{imageName}";
            File.WriteAllBytes(imagePath, imageData);

            using var imageStream = new MemoryStream(imageData);
            var image = Image.FromStream(imageStream);

            if (message.ErrorMessage != null)
            {
                _onConsoleWriteLine?.Invoke("Received snapshot. No embedded data. Error:");
                _onConsoleWriteLine?.Invoke(message.ErrorMessage);
                _onImageReceivedAndDataExtracted?.Invoke(clientId, image, string.Empty, message.ErrorMessage);
            }
            else
            {
                try
                {
                    var services = new ServiceCollection();
                    services.AddF5Services();
                    var serviceProvider = services.BuildServiceProvider();
                    var service = serviceProvider.GetService<IF5Service>();

                    var extractedMsg = ExtractMessage("password", imagePath, service);
                    _onConsoleWriteLine?.Invoke("Extracted message is: ");
                    _onConsoleWriteLine?.Invoke(extractedMsg);
                    _onImageReceivedAndDataExtracted?.Invoke(clientId, image, extractedMsg, string.Empty);
                }
                catch (Exception e)
                {
                    _onConsoleWriteLine?.Invoke(e.Message);
                    _onImageReceivedAndDataExtracted?.Invoke(clientId, image, string.Empty, e.Message);
                }
            }
        }

        public void UpdateInterval()
        {
            var interval = SnapshotRequestIntervalSeconds.Value * 1000;
            var result = _imageRequestTimer.Change(interval, interval);
        }
    }
}

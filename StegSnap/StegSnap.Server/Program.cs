using Microsoft.Extensions.DependencyInjection;
using StegSharp.Application.Common.Interfaces;
using StegSharp.Infrastructure;
using StegSnap.Domain.Common.Enums;
using StegSnap.Server.Common.Models;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;


namespace StegSnap.Server
{
    class Program
    {
        private static TcpListener _listener;
        private static readonly ConcurrentDictionary<Guid, TcpClient> _clients = new ConcurrentDictionary<Guid, TcpClient>();
        private static Timer _imageRequestTimer;
        private static int imageRequestInterval = 5000; // 5 seconds

        static async Task Main(string[] args)
        {
            int port = 3000;
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();

            Console.WriteLine($"Server started on port {port}");

            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected");
                _ = HandleClientAsync(client);
            }
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            var clientId = Guid.NewGuid();
            _clients.TryAdd(clientId, client);

            //inject f5 service
            // Create a new instance of the ServiceCollection class
            var services = new ServiceCollection();

            // Register the services provided by the class library projects
            services.AddF5Services();

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IF5Service>();


            try
            {
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream);
                using var writer = new StreamWriter(stream);

                await writer.WriteLineAsync(clientId.ToString());
                await writer.FlushAsync();


                _imageRequestTimer = new Timer(async _ =>
                {
                    foreach (var client in _clients)
                    {
                        await RequestImageFromClientAsync(clientId, writer);
                    }
                }, null, imageRequestInterval, imageRequestInterval);

                while (client.Connected)
                {
                    var messageJson = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(messageJson)) break;

                    var message = JsonSerializer.Deserialize<Message>(messageJson);
                    if (message == null) continue;

                    Console.WriteLine($"Received message from {clientId}: {message.Type}");

                    switch (message.Type)
                    {
                        case MessageType.UpdateIPAddress:
                            HandleUpdateIPAddress(clientId, message.Payload);
                            break;
                        case MessageType.SnapshotResponse:
                            var filePathToExtract = HandleReceivedImage(clientId, message.ImageData);
                            if(message.ErrorMessage != null)
                            {
                                Console.WriteLine("Received snapshot. No embedded data. Error:");
                                Console.WriteLine(message.ErrorMessage);
                            }
                            else
                            {
                                try
                                {
                                    serviceProvider = services.BuildServiceProvider();
                                    service = serviceProvider.GetService<IF5Service>();

                                    var extractedMsg = ExtractMessage("password", filePathToExtract, service);
                                    Console.WriteLine("Extracted message is: ");
                                    Console.WriteLine(extractedMsg);
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                            break;
                        // Add other message types and handling logic here
                        default:
                            Console.WriteLine($"Unknown message type: {message.Type}");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                _clients.TryRemove(clientId, out _);
                client.Close();
            }
        }

        private static void HandleUpdateIPAddress(Guid clientId, string payload)
        {
            // Update client's IP address in _clients, and notify other clients about the update
            // This is just a placeholder method; you need to implement the actual logic
        }
        private static string HandleReceivedImage(Guid clientId, byte[] imageData)
        {
            // Process the received image data, e.g., save it to a file
            var imageGuid = Guid.NewGuid();
            string imagePath = $"C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\ServerOutput\\received_image_{imageGuid}_from_{clientId}.jpg";
            File.WriteAllBytes(imagePath, imageData);

            Console.WriteLine($"Image {imageGuid} received from {clientId} and saved to {imagePath}");
            return imagePath;
        }

        private static async Task RequestImageFromClientAsync(Guid clientId, StreamWriter writer)
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
    }
}
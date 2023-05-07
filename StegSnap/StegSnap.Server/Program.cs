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

            try
            {
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream);
                using var writer = new StreamWriter(stream);

                await writer.WriteLineAsync(clientId.ToString());
                await writer.FlushAsync();

                await RequestImageFromClientAsync(clientId, writer);
                while (client.Connected)
                {
                    var messageJson = await reader.ReadLineAsync();
                    if (string.IsNullOrEmpty(messageJson)) break;

                    var message = JsonSerializer.Deserialize<Message>(messageJson);
                    if (message == null) continue;

                    Console.WriteLine($"Received message from {clientId}: {messageJson}");

                    switch (message.Type)
                    {
                        case MessageType.UpdateIPAddress:
                            HandleUpdateIPAddress(clientId, message.Payload);
                            break;
                        case MessageType.SnapshotResponse:
                            HandleReceivedImage(clientId, message.ImageData);
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
        private static void HandleReceivedImage(Guid clientId, byte[] imageData)
        {
            // Process the received image data, e.g., save it to a file
            string imagePath = $"received_image_from_{clientId}.jpg";
            File.WriteAllBytes(imagePath, imageData);
            Console.WriteLine($"Image received from {clientId} and saved to {imagePath}");
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
    }
}
using StegSnap.Domain.Common.Enums;
using StegSnap.Server.Common.Models;
using System.Net.Sockets;
using System.Text.Json;

namespace StegSnap.Client
{
    class Program
    {
        private static TcpClient _serverClient;
        private static Guid _clientId;
        private static NetworkStream _networkStream;
        private static StreamWriter _streamWriter;

        static async Task Main(string[] args)
        {
            string serverAddress = "127.0.0.1";
            int serverPort = 3000;

            _serverClient = new TcpClient();
            await _serverClient.ConnectAsync(serverAddress, serverPort);

            Console.WriteLine($"Connected to server at {serverAddress}:{serverPort}");

            _networkStream = _serverClient.GetStream();
            _streamWriter = new StreamWriter(_networkStream);

            _clientId = await RegisterWithServerAsync(_serverClient);

            Console.WriteLine($"Client ID: {_clientId}");

            await Task.Run(async () =>
            {
                using var reader = new StreamReader(_networkStream);
                while (_serverClient.Connected)
                {
                    var messageJson = await reader.ReadLineAsync();
                    var message = JsonSerializer.Deserialize<Message>(messageJson);

                    switch (message.Type)
                    {
                        case MessageType.SnapshotRequest:
                            string imagePath = "C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Images\\jo.jpg";
                            byte[] imageData = ReadImageFile(imagePath);
                            await SendImageToServerAsync(imageData);
                            break;
                            // Handle other message types here
                    }
                }
            });

            // Add application logic here, e.g., file sharing, chat, etc.

            // Example: Send an IP address update message to the server
            var message = new Message
            {
                Type = MessageType.UpdateIPAddress,
                Payload = "192.168.1.100:4000"
            };
            await SendMessageToServerAsync(message);
            //await SendImageToServerAsync(ReadImageFile("C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Images\\h.jpg"));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            // Properly close the StreamWriter and TcpClient before exiting
            _streamWriter.Dispose();
            _serverClient.Close();
        }

        private static async Task<Guid> RegisterWithServerAsync(TcpClient serverClient)
        {
            var reader = new StreamReader(_networkStream);
            var clientId = Guid.Parse(await reader.ReadLineAsync());
            //reader.Dispose(); // Dispose the StreamReader manually
            return clientId;
        }

        private static async Task SendMessageToServerAsync(Message message)
        {
            var messageJson = JsonSerializer.Serialize(message);
            await _streamWriter.WriteLineAsync(messageJson);
            await _streamWriter.FlushAsync();
        }

        private static byte[] ReadImageFile(string imagePath)
        {
            return File.ReadAllBytes(imagePath);
        }

        private static async Task SendImageToServerAsync(byte[] imageData)
        {
            var message = new Message
            {
                Type = MessageType.SnapshotResponse,
                ImageData = imageData
            };

            await SendMessageToServerAsync(message);
        }
    }
}
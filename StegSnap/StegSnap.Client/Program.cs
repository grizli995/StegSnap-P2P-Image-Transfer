using StegSnap.Domain.Common.Enums;
using StegSnap.Server.Common.Models;
using System.Net.Sockets;
using System.Text.Json;
using Emgu.CV;
using System.Drawing.Imaging;
using Emgu.CV.Structure;
using StegSharp.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using StegSharp.Application.Common.Interfaces;
using StegSharp.Application.Common.Exceptions;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

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

            //service.Embed(
            //    "C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Output\\snapshot_20230508_122825.jpg",
            //    "C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Output\\TEST---snapshot_20230508_122825.jpg",
            //    "password",
            //    "test");
            string serverAddress = "127.0.0.1";
            int serverPort = 3000;

            _serverClient = new TcpClient();
            await _serverClient.ConnectAsync(serverAddress, serverPort);

            Console.WriteLine($"Connected to server at {serverAddress}:{serverPort}");

            _networkStream = _serverClient.GetStream();
            _streamWriter = new StreamWriter(_networkStream);

            _clientId = await RegisterWithServerAsync(_serverClient);

            Console.WriteLine($"Client ID: {_clientId}");
            //inject f5 service
            // Create a new instance of the ServiceCollection class
            var services = new ServiceCollection();

            // Register the services provided by the class library projects
            services.AddF5Services();

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IF5Service>();

            await Task.Run(async () =>
            {
                using var reader = new StreamReader(_networkStream);
                while (_serverClient.Connected)
                {
                    var messageJson = await reader.ReadLineAsync();
                    if (!String.IsNullOrEmpty(messageJson))
                    {
                        var message = JsonSerializer.Deserialize<Message>(messageJson);
                        switch (message.Type)
                        {
                            case MessageType.SnapshotRequest:
                                string imagePath = CaptureImageFromCamera();
                                string embededImagePath;
                                string? errorMsg = null;
                                serviceProvider = services.BuildServiceProvider();
                                service = serviceProvider.GetService<IF5Service>();

                                try
                                {
                                    embededImagePath = EmbedHiddenData(imagePath, GetDiskTotalFreeSpaceInfo(), "password", service);
                                    Console.WriteLine($"Successfully embeded data in image {embededImagePath}");
                                }
                                catch (CapacityException ce)
                                {
                                    Console.WriteLine(ce.Message);
                                    embededImagePath = imagePath;
                                    errorMsg = ce.Message;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    embededImagePath = imagePath;
                                    errorMsg = e.Message;
                                }
                                byte[] imageData = ReadImageFile(embededImagePath);
                                await SendImageToServerAsync(imageData, errorMsg);
                                if (errorMsg == null)
                                    Console.WriteLine("Successfully sent embedded snapshot.");
                                break;
                                // Handle other message types here
                        }
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
            Console.WriteLine("UPDATE IP ADDRESS MSG SENT");
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

        private static async Task SendImageToServerAsync(byte[] imageData, string? errorMessage = null)
        {
            var message = new Message
            {
                Type = MessageType.SnapshotResponse,
                ImageData = imageData,
                ErrorMessage = errorMessage
            };

            await SendMessageToServerAsync(message);
        }

        private static string CaptureImageFromCamera()
        {
            using var cameraCapture = new VideoCapture();
            Mat frame = new Mat();

            cameraCapture.Read(frame);

            //var fileName = $"C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Output\\1.jpg";
            var fileName = $"C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Output\\snapshot_{DateTime.UtcNow.ToString("yyyyMMdd_hhmmss")}.jpg";
            frame.ToImage<Bgr, byte>().Save(fileName);

            return fileName;
        }

        private static string EmbedHiddenData(string fileName, string message, string password, IF5Service service)
        {
            var outPath = fileName.Substring(0, fileName.Length - 4) + "_embedded.jpg";

            service.Embed(fileName, outPath, password, message);

            return outPath;
        }


        static string GetDiskTotalFreeSpaceInfo()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            var result = string.Empty;
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady && drive.Name == "C:\\")
                {
                    result =  $"Remaining Space: {drive.TotalFreeSpace / (1024)} KB";
                }
            }

            return result;
        }
    }
}
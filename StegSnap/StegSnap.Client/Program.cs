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
using MethodTimer;

namespace StegSnap.Client
{
    class Program
    {
        private static TcpClient _serverClient;
        private static Guid _clientId;
        private static NetworkStream _networkStream;
        private static StreamWriter _streamWriter;
        private static string FilePathToRead;
        private static string DefaultPath = $"C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Output";
        private static string DefaultFilePathToRead = "C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\input.txt";

        static async Task Main(string[] args)
        {
            await SetupServerConnection();

            SetupInputFilePath();

            VideoCapture cam = CreateVideoCapture();
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
                                await HandleSnaphostRequestMessage(cam);
                                break;
                        }
                    }
                }
            });

            //// Example: Send an IP address update message to the server
            //var message = new Message
            //{
            //    Type = MessageType.UpdateIPAddress,
            //    Payload = "192.168.1.100:4000"
            //};
            //await SendMessageToServerAsync(message);
            //Console.WriteLine("UPDATE IP ADDRESS MSG SENT");
            ////await SendImageToServerAsync(ReadImageFile("C:\\Files\\Faks\\Faks\\Diplomski rad\\Implementacija\\StegSnap-P2P-Image-Transfer\\StegSnap\\Images\\h.jpg"));
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            // Properly close the StreamWriter and TcpClient before exiting
            _streamWriter.Dispose();
            _serverClient.Close();
        }

        private static void SetupInputFilePath()
        {
            Console.WriteLine("Please specify path to the txt input.");
            var path = Console.ReadLine();
            if (!String.IsNullOrEmpty(path))
            {
                FilePathToRead = path;
            }
        }

        private static async Task SetupServerConnection()
        {
            Console.WriteLine("Please enter the server IP address. (default 127.0.0.1)");
            string serverAddress = Console.ReadLine();
            if (String.IsNullOrEmpty(serverAddress))
                serverAddress = "127.0.0.1";

            int serverPort = 3000;
            _serverClient = new TcpClient();
            await _serverClient.ConnectAsync(serverAddress , serverPort);

            Console.WriteLine($"Connected to server at {serverAddress}:{serverPort}");

            _networkStream = _serverClient.GetStream();
            _streamWriter = new StreamWriter(_networkStream);

            _clientId = await RegisterWithServerAsync(_serverClient);

            Console.WriteLine($"Client ID: {_clientId}");
        }

        private static async Task<Guid> RegisterWithServerAsync(TcpClient serverClient)
        {
            var reader = new StreamReader(_networkStream);
            var clientId = Guid.Parse(await reader.ReadLineAsync());
            return clientId;
        }

        private static async Task SendMessageToServerAsync(Message message)
        {
            var messageJson = JsonSerializer.Serialize(message);
            await _streamWriter.WriteLineAsync(messageJson);
            await _streamWriter.FlushAsync();
        }

        [Time]
        private static byte[] ReadImageFile(string imagePath)
        {
            return File.ReadAllBytes(imagePath);
        }

        [Time]
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

        private static string CaptureImageFromCamera(VideoCapture cameraCapture)
        {
            Mat frame = new Mat();

            cameraCapture.Read(frame);

            var filePath = DefaultPath;
            var fileName = $"\\snapshot_{DateTime.UtcNow.ToString("yyyyMMdd_hhmmss")}.jpg";
            var fullName = $"{filePath}{fileName}";

            SaveImage(frame, fullName);

            return fullName;
        }

        private static void SaveImage(Mat frame, string fullName)
        {
            try
            {
                frame.ToImage<Bgr, byte>().Save(fullName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [Time]
        private static Mat CreateFrame()
        {
            return new Mat();
        }

        [Time]
        private static VideoCapture CreateVideoCapture()
        {
            return new VideoCapture();
        }

        [Time]
        private static string EmbedHiddenData(string fileName, string message, string password, IF5Service service)
        {
            var outPath = fileName.Substring(0, fileName.Length - 4) + "_embedded.jpg";

            service.Embed(fileName, outPath, password, message);

            return outPath;
        }

        [Time]
        static string ReadDataFromFile()
        {
            var filePath = FilePathToRead ?? DefaultFilePathToRead;
            string fileContent = "";

            try
            {
                // Open the text file using a stream reader.
                using (StreamReader reader = new StreamReader(filePath))
                {
                    // Read the entire file content.
                    fileContent = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur while reading the file.
                Console.WriteLine("Error occurred while reading the file: " + ex.Message);
            }

            return fileContent;
        }

        [Time]
        private static IF5Service? InjectF5Service()
        {
            //inject f5 service
            // Create a new instance of the ServiceCollection class
            var services = new ServiceCollection();

            // Register the services provided by the class library projects
            services.AddF5Services();

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IF5Service>();

            return service;
        }

        [Time]
        private async static Task HandleSnaphostRequestMessage(VideoCapture cam)
        {
            string embededImagePath;
            string? errorMsg = null;
            string imagePath = CaptureImageFromCamera(cam);
            var service = InjectF5Service();
            try
            {
                embededImagePath = EmbedHiddenData(imagePath, ReadDataFromFile(), "password", service);
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
        }
    }
}
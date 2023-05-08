using StegSnap.Domain.Common.Enums;

namespace StegSnap.Server.Common.Models
{
    public class Message
    {
        public MessageType Type { get; set; }
        public string Payload { get; set; }
        public byte[] ImageData { get; set; }

        public string? ErrorMessage { get; set; }
    }
}

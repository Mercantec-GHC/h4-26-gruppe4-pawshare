namespace Models
{
    public class MessageReadReceipt : Common
    {
        public required string MessageId { get; set; }
        public Message? Message { get; set; }

        public required string UserId { get; set; }
        public User? User { get; set; }
    }
}

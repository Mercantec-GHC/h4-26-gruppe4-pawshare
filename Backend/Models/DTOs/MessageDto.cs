
namespace Models.DTOs
{
    public class MessageDto
    {
        public required string MessageId { get; set; }
        public required string Content { get; set; }
        public required string SenderId { get; set; }
        public required DateTime CreatedAt { get; set; }
    }
}

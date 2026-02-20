namespace Models.DTOs
{
    public class UnreadChatDto
    {
        public required string ChatId { get; set; }
        public required int UnreadCount { get; set; }
    }
}


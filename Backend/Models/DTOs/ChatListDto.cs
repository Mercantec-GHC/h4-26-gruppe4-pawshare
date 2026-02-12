namespace Models.DTOs
{
    public class ChatListItemDto
    {
        public required string ChatId { get; set; }
        public required string Title { get; set; }

        public required MessagePreviewDto? NewestMessage { get; set; }

        public required List<ChatMemberDto> Members { get; set; }

        public required int UnreadCount { get; set; }
    }
}

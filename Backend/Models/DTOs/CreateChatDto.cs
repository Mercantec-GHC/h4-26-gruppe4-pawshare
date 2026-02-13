namespace Models.DTOs
{
    public class CreateChatDto
    {
        public required List<string> UserIds { get; set; }
        public required string Title { get; set; }
    }
}

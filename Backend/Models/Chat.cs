namespace Models
{
    public class Chat : Common
    {
        public required string Title { get; set; }
        public List<Message>? Messages { get; set; }
        public required List<ChatUserConvo> ChatUsers { get; set; }
    }
}

namespace Models
{
    public class Chat : Common
    {
        public required string Title { get; set; }
        public List<User>? Users { get; set; }
        public List<Message>? Messages { get; set; }
        public required List<ChatUserConvo> chatUsers { get; set; }
    }
}

namespace Models
{
    public class Post : Common
    {
        public required string Description { get; set; }
        public required bool IsRequest { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required string OwnerId { get; set; }
        public User? Owner { get; set; }
        public List<User>? Users { get; set; }
    }
}

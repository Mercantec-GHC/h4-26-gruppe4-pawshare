namespace Models
{
    public class User : Common
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string RealPassword { get; set; }
    }
}

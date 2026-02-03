
namespace Models.DTOs
{

    public class RegisterDto
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }

        public required string Base64Pfp { get; set; }
    }
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RefreshTokenDto
    {
        public required string RefreshToken { get; set; }
    }

}

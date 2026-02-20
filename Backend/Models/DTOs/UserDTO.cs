
namespace Models.DTOs
{

    public class UserDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Base64Pfp { get; set; } = null!;
    }
    public class RegisterDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string City { get; set; }


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

    public class AuthResponseDto
    {
        public string UserId { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
    public class LogoutDto
    {
        public string RefreshToken { get; set; } = null!;
    }

    public class RegisterOwnerDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string City { get; set; }
        public required string Base64Pfp { get; set; }

        public required string AnimalName { get; set; }
        public required string AnimalDescription { get; set; }
        public required DateOnly DateOfBirth { get; set; }
        public required string AnimalTypeId { get; set; }
    }

    public class RegisterInstitutionDto
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string City { get; set; }
        public required string Base64Pfp { get; set; }
    }

    public class ChangePasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}

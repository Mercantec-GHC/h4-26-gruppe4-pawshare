using Models;
using Models.DTOs;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Security.Cryptography;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _users;
        private readonly IJwtService _jwtService;
        private readonly IRoleRepo _roleRepo;
        private readonly IAnimalRepo _animalRepo;
        private readonly IEmailService _emailService;


        public AuthService(IUserRepo users, JwtService jwtService, IRoleRepo roleRepo, IAnimalRepo animalRepo, IEmailService emailService)
        {
            _users = users;
            _roleRepo = roleRepo;
            _jwtService = jwtService;
            _animalRepo = animalRepo;
            _emailService = emailService;
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


        public async Task Register(RegisterDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var role = await _roleRepo.GetByNameAsync("AnimalUser")
                ?? throw new Exception("Default role not found");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = dto.Name,
                Email = dto.Email,
                HashedPassword = hashedPassword,
                RoleId = role.Id,
                City = dto.City,

                
                ProfilePictureKey = dto.ProfilePictureKey
            };

            await _users.PostUser(user);
        }
        public async Task RegisterOwner(RegisterOwnerDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var role = await _roleRepo.GetByNameAsync("AnimalOwner")
                ?? throw new Exception("Role not found");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = dto.Name,
                Email = dto.Email,
                HashedPassword = hashedPassword,
                RoleId = role.Id,
                City = dto.City,
                ProfilePictureKey = dto.ProfilePictureKey,
            };

            await _users.PostUser(user);

            var animal = new Animal
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = dto.AnimalName,
                Description = dto.AnimalDescription,
                DateOfBirth = dto.DateOfBirth,
                TypeId = dto.AnimalTypeId,
                UserId = user.Id,
                AnimalPictureKey = ""
            };

            await _animalRepo.PostAnimal(animal);
            await _emailService.SendAsync(
               user.Email,
               "Welcome to PawShare!",
               $"Hi {user.Name},<br><br>Thank you for registering at PawShare! We're excited to have you on board.<br><br>Best regards,<br>The PawShare Team"
           );
        }

        public async Task RegisterInstitution(RegisterInstitutionDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var role = await _roleRepo.GetByNameAsync("Institution")
                ?? throw new Exception("Role not found");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Name = dto.Name,
                Email = dto.Email,
                HashedPassword = hashedPassword,
                RoleId = role.Id,
                City = dto.City,
                ProfilePictureKey = dto.ProfilePictureKey,
            };

            await _users.PostUser(user);

            await _emailService.SendAsync(
               user.Email,
               "Welcome to PawShare!",
               $"Hi {user.Name},<br><br>Thank you for registering at PawShare! We're excited to have you on board.<br><br>Best regards,<br>The PawShare Team"
           );
        }

        public async Task<AuthResponseDto?> Login(LoginDto dto)
        {
            var user = await _users.GetByEmail(dto.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
                return null;

            var refreshToken = GenerateRefreshToken();
            var refreshExpires = DateTime.UtcNow.AddDays(7);

            await _users.UpdateRefreshToken(
                user.Id,
                refreshToken,
                refreshExpires
            );

            var accessToken = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto?> RefreshAsync(string refreshToken)
        {
            var user = await _users.GetByRefreshTokenAsync(refreshToken);
            if (user == null)
                return null;

            if (user.RefreshTokenExpiresAt == null ||
                user.RefreshTokenExpiresAt < DateTime.UtcNow)
                return null;

            var newAccessToken = _jwtService.GenerateToken(user);

            var newRefreshToken = GenerateRefreshToken();
            var newRefreshExpires = DateTime.UtcNow.AddDays(7);

            await _users.UpdateRefreshToken(
                user.Id,
                newRefreshToken,
                newRefreshExpires
            );

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        public async Task<bool> LogoutAsync(string refreshToken)
        {
            var user = await _users.GetByRefreshTokenAsync(refreshToken);
            if (user == null)
                return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiresAt = null;

            await _users.UpdateUser(user);
            return true;
        }


        public async Task ForgotPassword(string email)
        {
            var user = await _users.GetByEmail(email);
            if (user == null)
                return;

            var resetToken = Guid.NewGuid().ToString();
            var resetExpires = DateTime.UtcNow.AddHours(1);

            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiresAt = resetExpires;

            await _users.UpdateUser(user);

            var resetLink = $"https://yourfrontend.com/reset-password?token={resetToken}";

            await _emailService.SendAsync(
                user.Email,
                "Password Reset Request",
                $"Hi {user.Name},<br><br>Click below:<br><a href='{resetLink}'>Reset Password</a>"
            );
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var user = await _users.GetByResetToken(token);
            if (user == null)
                return false;

            if (user.PasswordResetTokenExpiresAt == null ||
                user.PasswordResetTokenExpiresAt < DateTime.UtcNow)
                return false;

            // 🔐 BCrypt
            user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // 🔥 Інвалідовуємо токен
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiresAt = null;

            await _users.UpdateUser(user);

            return true;
        }
    }
}

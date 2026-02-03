using Models;
using Models.DTOs;
using Repositories.Interfaces;
using System.Security.Cryptography;

namespace Services
{
    public class AuthService
    {
        private readonly IUserRepo _users;
        private readonly JwtService _jwtService;

        
        public AuthService(IUserRepo users, JwtService jwtService)
        {
            _users = users;
            _jwtService = jwtService;
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


        public void Register(RegisterDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                // ===== Common fields =====
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                // ===== User fields =====
                Name = dto.Username,
                Email = dto.Email,
                HashedPassword = hashedPassword,

                // Required by the model but not used for authentication
                Salt = "BCrypt internal",
                RealPassword = "",

                // Required profile picture (Base64 encoded)
                Base64Pfp = dto.Base64Pfp
            };

            _users.PostUser(user);
        }

        public (string accessToken, string refreshToken)? Login(LoginDto dto)
        {
            var user = _users.GetByEmail(dto.Email);
            if (user == null)
                return null;

            var valid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.HashedPassword
            );

            if (!valid)
                return null;

            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            _users.UpdateUser(user); // або SaveChanges

            return (accessToken, refreshToken);
        }

        public string? Refresh(string refreshToken)
        {
            var user = _users.GetByRefreshToken(refreshToken);
            if (user == null)
                return null;

            if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
                return null;

            var newAccessToken = _jwtService.GenerateToken(user);

            return newAccessToken;
        }


    }
}

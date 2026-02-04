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
        private readonly IRoleRepo _roleRepo;

        
        public AuthService(IUserRepo users, JwtService jwtService, IRoleRepo roleRepo)
        {
            _users = users;
            _roleRepo = roleRepo;
            _jwtService = jwtService;
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

                // Required by the model but not used for authentication
                Salt = "BCrypt internal",
                RealPassword = dto.Password,
                Base64Pfp = dto.Base64Pfp
            };

            await _users.PostUser(user);
        }

        public async Task<AuthResponseDto?> Login(LoginDto dto)
        {
            var user = await _users.GetByEmail(dto.Email);
            if (user == null) return null;
 
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
                return null;

            var refreshToken = Guid.NewGuid().ToString();
            var refreshExpires = DateTime.UtcNow.AddDays(7);

            await _users.UpdateRefreshToken(
                user.Id,
                refreshToken,
                refreshExpires
            );

            var accessToken = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponseDto?> RefreshAsync(string refreshToken)
        {
            var user = await _users.GetByRefreshTokenAsync(refreshToken);
            if (user == null)
                return null;

            if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
                return null;

            var newAccessToken = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken
            };
        }


    }
}

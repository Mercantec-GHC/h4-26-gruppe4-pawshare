using Services.Interfaces;
using Models;
using Repositories.Interfaces;
using Models.DTOs;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IRoleRepo _roleRepo;

    public UserService(IUserRepo userRepo, IRoleRepo roleRepo)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
    }
    public async Task<UserDto?> GetUser(string id)
    {
        var user = await _userRepo.GetUser(id);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Base64Pfp = user.Base64Pfp
        };
    }

    public async Task Register(RegisterDto dto)
    {
        var role = await _roleRepo.GetByNameAsync("AnimalUser")
            ?? throw new Exception("Default role not found");

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Name = dto.Name,
            Email = dto.Email,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Salt = "BCrypt internal",
            Base64Pfp = "",
            RoleId = role.Id
        };

        await _userRepo.PostUser(user);
    }
}
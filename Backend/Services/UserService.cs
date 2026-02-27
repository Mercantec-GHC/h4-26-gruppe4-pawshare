using Services.Interfaces;
using Models;
using Repositories.Interfaces;
using Models.DTOs;

namespace Services;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    private readonly IRoleRepo _roleRepo;
    private readonly IMediaService _mediaService;

    public UserService(IUserRepo userRepo, IRoleRepo roleRepo, IMediaService mediaService)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _mediaService = mediaService;
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
            ProfilePictureKey = user.ProfilePictureKey
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
            ProfilePictureKey = dto.ProfilePictureKey,
            RoleId = role.Id,
            City = dto.City
        };

        await _userRepo.PostUser(user);
    }

    public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await _userRepo.GetUser(userId);
        if (user == null)
            return false;

        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.HashedPassword))
            return false;

        var newHashed = BCrypt.Net.BCrypt.HashPassword(newPassword);

        user.HashedPassword = newHashed;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepo.UpdateUser(user);
        return true;
    }

    public async Task<bool> UpdateProfilePictureAsync(string userId, string newProfilePictureKey, string? oldProfilePictureKey = null)
    {
        var user = await _userRepo.GetUser(userId);
        if (user == null)
        {
            return false;
        }

        // Delete old picture if it exists
        if (!string.IsNullOrWhiteSpace(oldProfilePictureKey))
        {
            await _mediaService.DeleteFileAsync(oldProfilePictureKey);
        }

        // Update user with new picture key
        user.ProfilePictureKey = newProfilePictureKey;
        user.UpdatedAt = DateTime.UtcNow;

        var updated = await _userRepo.UpdateUser(user);
        if (updated == null)
        {
            return false;
        }

        return true;
    }
}
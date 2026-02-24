using Models;
using Models.DTOs;

namespace Services.Interfaces;

public interface IUserService
{
    public Task<UserDto?> GetUser(string id);
    Task Register(RegisterDto dto);
    public Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

    /// <summary>
    /// Updates the profile picture key for a user and optionally deletes the old picture.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="newProfilePictureKey">The new profile picture object key.</param>
    /// <param name="oldProfilePictureKey">The old profile picture object key to delete (nullable).</param>
    /// <returns>True if successful, false otherwise.</returns>
    Task<bool> UpdateProfilePictureAsync(string userId, string newProfilePictureKey, string? oldProfilePictureKey = null);
}
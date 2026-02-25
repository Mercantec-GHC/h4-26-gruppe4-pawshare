using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    
    
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.GetUser(userId);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var success = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

        if (!success)
            return BadRequest();

        return Ok();
    }

    [HttpPost("profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture(UpdateProfilePictureDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(dto.NewProfilePictureKey))
            return BadRequest(new { error = "Profile picture key is required" });

        var success = await _userService.UpdateProfilePictureAsync(
            userId,
            dto.NewProfilePictureKey,
            dto.OldProfilePictureKey
        );

        if (!success)
            return BadRequest(new { error = "Failed to update profile picture" });

        return Ok();
    }

}
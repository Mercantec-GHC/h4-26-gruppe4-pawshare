using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Services;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            await _auth.Register(dto);
            return Created("", null);
        }

        [HttpPost("register-owner")]
        public async Task<IActionResult> RegisterOwner(RegisterOwnerDto dto)
        {
            await _auth.RegisterOwner(dto);
            return Ok("Owner created");
        }

        [HttpPost("register-institution")]
        public async Task<IActionResult> RegisterInstitution(RegisterInstitutionDto dto)
        {
            await _auth.RegisterInstitution(dto);
            return Ok("Institution created");
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
        {
            var result = await _auth.Login(dto);
            if (result == null)
                return Unauthorized();

            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshTokenDto dto)
        {
            var result = await _auth.RefreshAsync(dto.RefreshToken);
            if (result == null)
                return Unauthorized();

            return Ok(result);
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutDto dto)
        {
            var success = await _auth.LogoutAsync(dto.RefreshToken);
            if (!success)
                return Unauthorized();

            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (userId == null)
                return Unauthorized();

            return Ok(new
            {
                UserId = userId,
                Email = email
            });
        }




        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            await _auth.ForgotPassword(dto.Email);
            return Ok("Password reset email sent");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _auth.ResetPassword(dto.Token, dto.NewPassword);

            if (!result)
                return BadRequest("Invalid or expired token");

            return Ok();
        }
    }
}

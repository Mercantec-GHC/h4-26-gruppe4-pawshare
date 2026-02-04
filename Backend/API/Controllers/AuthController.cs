using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTOs;
using Services;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            _auth.Register(dto);
            return Ok("User created");
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


    }

}

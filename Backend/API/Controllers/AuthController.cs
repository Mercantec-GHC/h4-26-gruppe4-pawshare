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
        public IActionResult Login(LoginDto dto)
        {
            var result = _auth.Login(dto);

            if (result == null)
                return Unauthorized();

            return Ok(new
            {
                accessToken = result.Value.accessToken,
                refreshToken = result.Value.refreshToken
            });
        }

        [HttpPost("refresh")]
        public IActionResult Refresh(RefreshTokenDto dto)
        {
            var newToken = _auth.Refresh(dto.RefreshToken);

            if (newToken == null)
                return Unauthorized();

            return Ok(new { accessToken = newToken });
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

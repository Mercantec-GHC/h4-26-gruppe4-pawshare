using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Models;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        return await _userService.GetUser(id);
    }
}
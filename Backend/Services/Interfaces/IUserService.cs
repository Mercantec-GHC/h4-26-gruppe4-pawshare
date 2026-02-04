using Models;
using Models.DTOs;

namespace Services.Interfaces;

public interface IUserService
{
    public Task<UserDto?> GetUser(string id);
    Task Register(RegisterDto dto);
}
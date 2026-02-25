using Models.DTOs;

namespace Services.Interfaces;
public interface IAuthService
{
    Task Register(RegisterDto dto);
    Task ForgotPassword(string email);
}
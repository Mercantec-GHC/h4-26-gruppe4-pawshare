using System;
using Models.DTOs;

namespace Services.Interfaces;

public interface IAuthService
{
    public Task Register(RegisterDto dto);

    public Task RegisterOwner(RegisterOwnerDto dto);

    public Task RegisterInstitution(RegisterInstitutionDto dto);

    public Task<AuthResponseDto?> Login(LoginDto dto);

    public Task<AuthResponseDto?> RefreshAsync(string refreshToken);

    public Task<bool> LogoutAsync(string refreshToken);


}

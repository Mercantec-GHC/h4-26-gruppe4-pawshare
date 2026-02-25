using System;
using Models;

namespace Services.Interfaces;

public interface IJwtService
{
    public string GenerateToken(User user);
}

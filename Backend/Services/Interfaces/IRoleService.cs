using Models;

namespace Services.Interfaces;

public interface IRoleService
{
    public Task<Role?> GetRole(string id);
}
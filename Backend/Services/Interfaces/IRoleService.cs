using Models;

namespace Services.Interfaces;

public interface IRoleService
{
    Task<Role?> GetRoleByNameAsync(string name);
    Task<IEnumerable<Role>> GetAllRolesAsync();
}
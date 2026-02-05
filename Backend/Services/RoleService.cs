using Services.Interfaces;
using Repositories.Interfaces;
using Models;

namespace Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepo _roleRepo;

    public RoleService(IRoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }

    public async Task<Role?> GetRoleByNameAsync(string name)
    {
        return await _roleRepo.GetByNameAsync(name);
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        return await _roleRepo.GetAllAsync();
    }
}
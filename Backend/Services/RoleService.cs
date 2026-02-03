using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepo _roleRepo;

    public RoleService(IRoleRepo roleRepo)
    {
        _roleRepo = roleRepo;
    }
    public Task<Role?> GetRole(string id)
    {
        return _roleRepo.GetRole(id);
    }
}
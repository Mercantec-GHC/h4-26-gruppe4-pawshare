using Microsoft.EntityFrameworkCore;
using Repositories.Context;
using Repositories.Interfaces;
using Models;


namespace Repositories;

public class RoleRepo : IRoleRepo
{
    private readonly AppDBContext _dbContext;

    public RoleRepo(AppDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        return await _dbContext.Roles.ToListAsync();
    }
}
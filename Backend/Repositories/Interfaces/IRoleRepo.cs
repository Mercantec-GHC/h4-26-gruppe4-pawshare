using Models;

namespace Repositories.Interfaces;

public interface IRoleRepo
{
    Task<Role?> GetByNameAsync(string name);
    Task<IEnumerable<Role>> GetAllAsync();
}
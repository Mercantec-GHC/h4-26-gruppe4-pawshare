namespace Repositories.Interfaces;
using Models;

public interface IUserRepo
{
    public Task<User> GetUser(string id);
}
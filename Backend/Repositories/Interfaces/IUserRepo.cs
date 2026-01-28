namespace Repositories.Interfaces;
using Models;

public interface IUserRepo
{
    public Task<User> GetUser(string id);

    User? GetByEmail(string email);
    void Add(User user);
}
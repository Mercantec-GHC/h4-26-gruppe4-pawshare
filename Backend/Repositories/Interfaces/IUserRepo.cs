namespace Repositories.Interfaces;
using Models;

public interface IUserRepo
{
    public Task<User> GetUser(string id);
    public Task<User> PostUser(User newUser);
    public Task<List<User>> GetAllUsers();
}
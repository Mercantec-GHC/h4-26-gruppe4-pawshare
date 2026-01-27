using Models;

namespace Services.Interfaces;

public interface IUserService
{
    public Task<User> GetUser(string id);
}
using Repositories.Context;
using Repositories.Interfaces;

namespace Repositories;
using Models;


public class UserRepo : IUserRepo
{
    private readonly AppDBContext _dbContext;

    public UserRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<User> GetUser(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if ( user is null)
        {
            return null;
        }

        return user;
    }

}
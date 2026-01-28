using Repositories.Context;
using Repositories.Interfaces;

namespace Repositories;

using Microsoft.EntityFrameworkCore;
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

    public User? GetByEmail(string email)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Email == email);
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

}
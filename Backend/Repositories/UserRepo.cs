using Repositories.Context;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq.Expressions;

namespace Repositories;

public class UserRepo : IUserRepo
{
    private readonly AppDBContext _dbContext;

    public UserRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public async Task<List<User>> GetAllUsers(Expression<Func<User, bool>>? filter = null)
    {
        IQueryable<User> query = _dbContext.Users.AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<User?> GetUser(string id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        if ( user is null)
        {
            return null;
        }

        return user;
    }

    /// <inheritdoc/>
    public User? GetByEmail(string email)
    {
        try
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user is null)
            {
                return null;
            }

            return user;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <inheritdoc/>
    public async Task<User?> PostUser(User newUser)
    {
        _dbContext.Users.Add(newUser);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Users.Any(e => e.Id == newUser.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newUser;
    }

    /// <inheritdoc/>
    public Task<User?> UpdateUser(User User)
    {
        throw new NotImplementedException();
    }
    
    /// <inheritdoc/>
    public Task<bool> DeleteUser(string UserId)
    {
        throw new NotImplementedException();
    }
}
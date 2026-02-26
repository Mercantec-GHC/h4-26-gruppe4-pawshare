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

    public async Task<List<User>> GetAllUsers(Expression<Func<User, bool>>? filter = null)
    {
        IQueryable<User> query = _dbContext.Users.AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }


    public async Task<User?> GetUser(string id)
    {
        return await _dbContext.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmail(string email)
    {
        try
        {
            var user = await _dbContext.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.Email == email);
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

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _dbContext.Users
        .Include(u => u.Role)
        .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }

    public async Task<User?> GetByResetToken(string token)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.PasswordResetToken == token);
    }
    
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

        public async Task<User?> UpdateUser(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task UpdateRefreshToken(
    string userId,
    string refreshToken,
    DateTime expiresAt)
    {
        await _dbContext.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.RefreshToken, refreshToken)
                .SetProperty(u => u.RefreshTokenExpiresAt, expiresAt)
                .SetProperty(u => u.UpdatedAt, DateTime.UtcNow)
            );
    }

    public async Task<bool> DeleteUser(string UserId)
    {
        var user = await _dbContext.Users.FindAsync(UserId);
        if (user is null)
        {
            return false;
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Context;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

public class RoleRepo : IRoleRepo
{
    private readonly AppDBContext _dbContext;

    public RoleRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public async Task<List<Role>> GetAllRoles(Expression<Func<Role, bool>>? filter = null)
    {
        IQueryable<Role> query = _dbContext.Roles.AsQueryable();

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Role?> GetRole(string id)
    {
        var role = await _dbContext.Roles.FindAsync(id);
        if (role is null)
        {
            return null;
        }

        return role;
    }

    /// <inheritdoc/>
    public async Task<Role?> PostRole(Role newRole)
    {
        _dbContext.Roles.Add(newRole);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Roles.Any(e => e.Id == newRole.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newRole;
    }

    /// <inheritdoc/>
    public async Task<Role?> UpdateRole(Role newRole)
    {
        _dbContext.Entry(newRole).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_dbContext.Roles.Any(e => e.Id == newRole.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newRole;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteRole(string roleId)
    {
        Role? role = await _dbContext.Roles.FindAsync(roleId);
        if (role == null)
        {
            return false;
        }

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}

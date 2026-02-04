using Repositories.Context;
using Repositories.Interfaces;
namespace Repositories;

using Microsoft.EntityFrameworkCore;
using Models;


public class ChatRepo : IChatRepo
{
    private readonly AppDBContext _dbContext;

    public ChatRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public async Task<List<Chat>> GetChatsWithUser(string userId)
    {
        return await _dbContext.Chats
            .Include(c => c.ChatUsers)
            .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId))
            .ToListAsync();
    }
    /// <inheritdoc/>
    public async Task<Chat?> PostChat(Chat newChat)
    {
        _dbContext.Chats.Add(newChat);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Chats.Any(e => e.Id == newChat.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newChat;
    }

    /// <inheritdoc/>
    public async Task<Chat?> GetChat(string id)
    {
        return await _dbContext.Chats
            .Include(c => c.ChatUsers)
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    /// <inheritdoc/>
    public async Task<Chat?> UpdateChat(Chat newChat)
    {
        _dbContext.Entry(newChat).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_dbContext.Chats.Any(e => e.Id == newChat.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newChat;
    }
}
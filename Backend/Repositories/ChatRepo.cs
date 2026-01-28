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
        List<Chat>? chats = _dbContext.Chats.Where(e => e.Users != null && e.Users.Any(user => user.Id == userId)).ToList();

        if (chats is null)
        {
            return [];
        }

        return chats;
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
}
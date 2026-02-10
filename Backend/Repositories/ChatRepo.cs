using Repositories.Context;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories;


public class ChatRepo : IChatRepo
{
    private readonly AppDBContext _dbContext;

    public ChatRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public IQueryable<Chat> Query() => _dbContext.Chats.AsQueryable();

    /// <inheritdoc/>
    public async Task<Chat?> PostChat(Chat newChat)
    {
        _dbContext.Chats.Add(newChat);
        await _dbContext.SaveChangesAsync();
        return newChat;
    }

    /// <inheritdoc/>
    public async Task<Chat?> GetChat(string id)
    {
        return await _dbContext.Chats.FindAsync(id);
    }
    /// <inheritdoc/>
    public async Task<Chat?> UpdateChat(Chat newChat)
    {
        _dbContext.Entry(newChat).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return newChat;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteChat(string id) { 
        var chat = await _dbContext.Chats.FindAsync(id); 
        if (chat is null) 
            return false; 
        
        _dbContext.Chats.Remove(chat); 
        await _dbContext.SaveChangesAsync(); 
        return true; 
    }

    /// <inheritdoc/>
    public async Task<bool> IsUserInChat(string chatId, string userId)
    {
        return await _dbContext.Chats
            .AnyAsync(c => c.Id == chatId && c.ChatUsers.Any(cu => cu.UserId == userId));
    }
}
using Repositories.Context;
using Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;


namespace Repositories;


public class MessageRepo : IMessageRepo
{
    private readonly AppDBContext _dbContext;

    public MessageRepo(AppDBContext dBContext)
    {
        _dbContext = dBContext;
    }

    /// <inheritdoc/>
    public IQueryable<Message> Query() => _dbContext.Messages.AsQueryable();

    /// <inheritdoc/>
    public async Task<Message?> SendMessage(Message newMessage)
    {
        _dbContext.Messages.Add(newMessage);
        await _dbContext.SaveChangesAsync();
        return newMessage;
    }

    /// <inheritdoc/>
    public async Task<Message?> UpdateMessage(Message NewMessage)
    {
        _dbContext.Entry(NewMessage).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return NewMessage;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteMessage(string MessageId)
    {
        Message? message = await _dbContext.Messages.FindAsync(MessageId);
        if (message == null)
            return false;

        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
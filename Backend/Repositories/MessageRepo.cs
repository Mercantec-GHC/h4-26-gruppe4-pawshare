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
    public async Task<List<Message>> GetMessagesFromChat(string chatId)
    {
        List<Message> messages = await _dbContext.Messages.Where(e => e.ChatId.Equals(chatId)).ToListAsync();
        if (messages is null)
        {
            return [];
        }

        return messages;
    }

    /// <inheritdoc/>
    public async Task<Message?> SendMessage(Message newMessage)
    {
        _dbContext.Messages.Add(newMessage);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (_dbContext.Messages.Any(e => e.Id == newMessage.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return newMessage;
    }

    /// <inheritdoc/>
    public async Task<Message?> UpdateMessage(Message NewMessage)
    {
        _dbContext.Entry(NewMessage).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_dbContext.Messages.Any(e => e.Id == NewMessage.Id))
            {
                return null;
            }
            else
            {
                throw;
            }
        }

        return NewMessage;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteMessage(string MessageId)
    {
        Message? message = await _dbContext.Messages.FindAsync(MessageId);
        if (message == null)
        {
            return false;
        }

        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
using Repositories.Context;
using Repositories.Interfaces;

namespace Repositories;

using Microsoft.EntityFrameworkCore;
using Models;


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
        List<Message> messages = _dbContext.Messages.Where(e => e.ChatId.Equals(chatId)).ToList();
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
}
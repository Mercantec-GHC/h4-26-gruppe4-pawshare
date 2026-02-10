using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepo _messageRepo;
    private readonly IMessageReadReceiptRepo _receiptRepo;

    public MessageService(IMessageRepo messageRepo, IMessageReadReceiptRepo receiptRepo)
    {
        _messageRepo = messageRepo;
        _receiptRepo = receiptRepo;
    }

    public async Task<bool> MarkMessageReadAsync(string messageId, string userId)
    {
        // Get message
        bool exists = await _messageRepo.Query().AnyAsync(m => m.Id == messageId);
        if (!exists)
            return false;

        // Check if message was already read
        if (await _receiptRepo.ExistsAsync(messageId, userId)) 
            return true; 
        
        // Create new receipt
        var receipt = new MessageReadReceipt { 
            Id = Guid.NewGuid().ToString(), 
            MessageId = messageId, 
            UserId = userId, 
            CreatedAt = DateTime.UtcNow, 
            UpdatedAt = DateTime.UtcNow 
        }; 
        
        await _receiptRepo.PostReceiptAsync(receipt); 
        return true; 
    }
}

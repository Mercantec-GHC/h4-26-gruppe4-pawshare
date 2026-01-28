using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepo _messageRepo;
    private readonly IChatRepo _chatRepo;
    private readonly IUserRepo _userRepo;

    public MessageService(IMessageRepo messageRepo, IChatRepo chatRepo, IUserRepo userRepo)
    {
        _messageRepo = messageRepo;
        _chatRepo = chatRepo;
        _userRepo = userRepo;
    }

    public async Task<List<Message>> GetMessagesByChatAsync(string chatId)
    {
        return await _messageRepo.GetMessagesFromChat(chatId);
    }

    public async Task<Message?> SendMessageAsync(string chatId, string userId, string content)
    {
        var user = await _userRepo.GetUser(userId);
        if (user == null) return null;

        var message = new Message
        {
            Id = Guid.NewGuid().ToString(),
            Content = content,
            UserId = userId,
            User = user,
            ChatId = chatId,
            Chat = null!,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _messageRepo.SendMessage(message);
    }

    public async Task<bool> DeleteMessageAsync(string id)
    {
        // Simple check - actual delete logic would go in repo
        return true;
    }
}

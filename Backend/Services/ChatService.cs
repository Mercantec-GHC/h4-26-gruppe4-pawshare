using Services.Interfaces;
using Models;
using Repositories.Interfaces;

namespace Services;

public class ChatService : IChatService
{
    private readonly IChatRepo _chatRepo;
    private readonly IMessageRepo _messageRepo;
    private readonly IUserRepo _userRepo;

    public ChatService(IChatRepo chatRepo, IMessageRepo messageRepo, IUserRepo userRepo)
    {
        _chatRepo = chatRepo;
        _messageRepo = messageRepo;
        _userRepo = userRepo;
    }

    public async Task<Chat?> GetChatAsync(string id)
    {
        return await _chatRepo.GetChat(id);
    }

    public async Task<Chat> CreateChatAsync(string title, List<User> users)
    {
        var chatId = Guid.NewGuid().ToString();
        var chat = new Chat
        {
            Id = chatId,
            Title = title,
            Messages = new List<Message>(),
            ChatUsers = users.Select(u => new ChatUserConvo
            {
                ChatId = chatId,
                UserId = u.Id,

            }).ToList(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return (await _chatRepo.PostChat(chat))!;
    }

    public async Task<List<Chat>> GetUserChatsAsync(string userId)
    {
        return await _chatRepo.GetChatsWithUser(userId);
    }

    public async Task<Chat?> AddUserToChatAsync(string chatId, string userId)
    {
        var user = await _userRepo.GetUser(userId);
        if (user == null) return null;

        var chat = await _chatRepo.GetChat(chatId);
        if (chat == null) return null;

        chat.ChatUsers ??= new List<ChatUserConvo>();

        if (!chat.ChatUsers.Any(cu => cu.UserId == userId))
        {
            chat.ChatUsers.Add(new ChatUserConvo
            {
                ChatId = chatId,
                UserId = userId
            });
        }

        chat.UpdatedAt = DateTime.UtcNow;
        return await _chatRepo.UpdateChat(chat);
    }

    public async Task<Chat?> RemoveUserFromChatAsync(string chatId, string userId)
    {
        var chat = await _chatRepo.GetChat(chatId);
        if (chat == null) return null;

        chat.ChatUsers?.RemoveAll(cu => cu.UserId == userId);
        chat.UpdatedAt = DateTime.UtcNow;

        return await _chatRepo.UpdateChat(chat);
    }

    public async Task<List<Message>> GetChatMessagesAsync(string chatId)
    {
        return await _messageRepo.GetMessagesFromChat(chatId);
    }

    public async Task<Chat?> SendMessageAsync(string chatId, string senderId, string content)
    {
        var user = await _userRepo.GetUser(senderId);
        if (user == null) return null;

        var chat = await _chatRepo.GetChat(chatId);
        if (chat == null) return null;

        var message = new Message
        {
            Id = Guid.NewGuid().ToString(),
            Content = content,
            UserId = senderId,
            User = user,
            ChatId = chatId,
            Chat = chat,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _messageRepo.SendMessage(message);
        chat.UpdatedAt = DateTime.UtcNow;

        return await _chatRepo.UpdateChat(chat);
    }
}

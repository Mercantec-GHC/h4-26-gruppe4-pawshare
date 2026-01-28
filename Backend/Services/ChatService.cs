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
        var chats = await _chatRepo.GetChatsWithUser(id);
        return chats.FirstOrDefault(c => c.Id == id);
    }

    public async Task<Chat> CreateChatAsync(string title, List<User> users)
    {
        var chat = new Chat
        {
            Id = Guid.NewGuid().ToString(),
            Title = title,
            Users = users,
            Messages = new List<Message>(),
            chatUsers = users.Select(u => new ChatUserConvo
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = 0,
                UserId = int.Parse(u.Id),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }).ToList(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return await _chatRepo.PostChat(chat);
    }

    public async Task<List<Chat>> GetUserChatsAsync(string userId)
    {
        return await _chatRepo.GetChatsWithUser(userId);
    }

    public async Task<Chat?> AddUserToChatAsync(string chatId, string userId)
    {
        var user = await _userRepo.GetUser(userId);
        if (user == null) return null;

        var chats = await _chatRepo.GetChatsWithUser(chatId);
        var chat = chats.FirstOrDefault(c => c.Id == chatId);
        if (chat == null) return null;

        chat.Users ??= new List<User>();
        chat.Users.Add(user);
        chat.UpdatedAt = DateTime.UtcNow;

        return chat;
    }

    public async Task<Chat?> RemoveUserFromChatAsync(string chatId, string userId)
    {
        var chats = await _chatRepo.GetChatsWithUser(chatId);
        var chat = chats.FirstOrDefault(c => c.Id == chatId);
        if (chat == null) return null;

        chat.Users?.RemoveAll(u => u.Id == userId);
        chat.UpdatedAt = DateTime.UtcNow;

        return chat;
    }

    public async Task<List<Message>> GetChatMessagesAsync(string chatId)
    {
        return await _messageRepo.GetMessagesFromChat(chatId);
    }

    public async Task<Chat?> SendMessageAsync(string chatId, string senderId, string content)
    {
        var user = await _userRepo.GetUser(senderId);
        if (user == null) return null;

        var chats = await _chatRepo.GetChatsWithUser(chatId);
        var chat = chats.FirstOrDefault(c => c.Id == chatId);
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

        return chat;
    }
}

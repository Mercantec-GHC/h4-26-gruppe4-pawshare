using Mappers;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services;

public class ChatService : IChatService
{
    private readonly IChatRepo _chatRepo; 
    private readonly IMessageRepo _messageRepo; 
    private readonly IUserRepo _userRepo;
    private readonly IMessageReadReceiptRepo _receiptRepo; 

    public ChatService(IChatRepo chatRepo, IMessageRepo messageRepo, IUserRepo userRepo, IMessageReadReceiptRepo receiptRepo)
    {
        _chatRepo = chatRepo;
        _messageRepo = messageRepo;
        _userRepo = userRepo;
        _receiptRepo = receiptRepo;
    }

    public async Task<ChatListItemDto?> CreateChatAsync(CreateChatDto dto) {
        var chatId = Guid.NewGuid().ToString();
        List<ChatUserConvo> convos = [];

        foreach(string id in dto.UserIds) {
            // Checks all users exist
            var user = await _userRepo.GetUser(id);
            if (user is null)
                continue;

            //Creates a list of ChatUserConvo models
            convos.Add(new ChatUserConvo { ChatId = chatId, UserId = id });    
        }

        if (convos.Count < 1)
            return null;


        var chat = new Chat { 
            Id = chatId, 
            Title = dto.Title, 
            CreatedAt = DateTime.UtcNow, 
            UpdatedAt = DateTime.UtcNow, 
            ChatUsers = convos,
            Messages = new List<Message>() 
        }; 
        
        await _chatRepo.PostChat(chat); 
        return ChatMapper.ToListItemDto(chat, newestMessage: null, unreadCount: 0); 
    }

    public async Task<List<ChatListItemDto>> GetChatsForUserAsync(string userId, int? limit, int? offset) {

        // Gets queryable chats sorted by creation date
        IQueryable<Chat> query = _chatRepo.Query()
            .Include(c => c.ChatUsers)
                .ThenInclude(cu => cu.User)
            .Include(c => c.Messages)!
                .ThenInclude(m => m.ReadReceipts)
            .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId))
            .OrderByDescending(c => c.Messages!.Any()
                ? c.Messages!.Max(m => m.CreatedAt)
                : c.CreatedAt);

        // Skips given offset
        if (offset is not null) query = query.Skip(offset.Value);

        // Limits to given value
        if (limit is not null) query = query.Take(limit.Value);


        var chats = await query.ToListAsync();
        return chats.Select(c => {
            var newest = c.Messages!.OrderByDescending(m => m.CreatedAt).FirstOrDefault();
            var unread = c.Messages!.Count(m => !m.ReadReceipts!.Any(r => r.UserId == userId));
            
            return ChatMapper.ToListItemDto(c, newest, unread);
        }).ToList();
    }

    public async Task<List<MessageDto>> GetMessagesAsync(string chatId, int? limit, int? offset) {
        IQueryable<Message> query = _messageRepo.Query().Where(m => m.ChatId == chatId).OrderByDescending(m => m.CreatedAt);
        
        // Skips given offset
        if (offset is not null) query = query.Skip(offset.Value);

        // Limits to given value
        if (limit is not null) query = query.Take(limit.Value);
        
        var messages = await query.ToListAsync();
        return messages.Select(MessageMapper.ToDto).ToList();
    }

    public async Task<bool> SendMessageAsync(string chatId, string userId, string content)
    {
        // Check membership
        if (!await _chatRepo.IsUserInChat(chatId, userId))
            return false;

        // Check user exists
        var user = await _userRepo.GetUser(userId);
        if (user is null)
            return false;

        // Construct message
        var message = new Message
        {
            Id = Guid.NewGuid().ToString(),
            ChatId = chatId,
            UserId = userId,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Sends message and updates UpdatedAt date in chat
        await _messageRepo.SendMessage(message);
        var chat = await _chatRepo.GetChat(chatId);
        chat!.UpdatedAt = DateTime.UtcNow;
        await _chatRepo.UpdateChat(chat);

        return true;
    }


    public async Task<List<UnreadChatDto>> GetUnreadChatsAsync(string userId) {
        // GEts queryable chats from table
        var chats = await _chatRepo.Query()
            .Include(c => c.Messages)!
                .ThenInclude(m => m.ReadReceipts)
            .Include(c => c.ChatUsers)
            .Where(c => c.ChatUsers.Any(cu => cu.UserId == userId))
            .ToListAsync();
        
        // Returns a list of dtos
        return chats.Select(c => new UnreadChatDto {
            ChatId = c.Id,
            UnreadCount = c.Messages!.Count(m => !m.ReadReceipts!.Any(r => r.UserId == userId))
        }).ToList();
    }
}

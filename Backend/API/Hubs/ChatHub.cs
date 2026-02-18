using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Models.DTOs;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IMessageService _messageService;
    private readonly IChatRepo _chatRepo;

    public ChatHub(IChatService chatService, IMessageService messageService, IChatRepo chatRepo)
    {
        _chatService = chatService;
        _messageService = messageService;
        _chatRepo = chatRepo;
    }

    public async Task<ChatListItemDto?> CreateChat(CreateChatDto dto)
    {
        return await _chatService.CreateChatAsync(dto);
    }

    public async Task<List<ChatListItemDto>> GetChats()
    {
        var userId = GetUserId();
        return await _chatService.GetChatsForUserAsync(userId, null, null);
    }

    public async Task<List<MessageDto>> GetMessages(string chatId)
    {
        EnsureChatId(chatId);
        return await _chatService.GetMessagesAsync(chatId, null, null);
    }

    public async Task SendMessage(string chatId, string content)
    {
        EnsureChatId(chatId);
        var userId = GetUserId();

        var sent = await _chatService.SendMessageAsync(chatId, userId, content);
        if (!sent)
            throw new HubException("Unable to send message.");

        var newestMessage = (await _chatService.GetMessagesAsync(chatId, 1, 0)).FirstOrDefault();
        if (newestMessage is null)
            throw new HubException("Message was sent but could not be loaded.");

        await Clients.Group(GetChatGroupName(chatId)).SendAsync("ReceiveMessage", new
        {
            ChatId = chatId,
            newestMessage.MessageId,
            newestMessage.Content,
            newestMessage.SenderId,
            newestMessage.CreatedAt
        });
    }

    public async Task MarkRead(string messageId)
    {
        if (string.IsNullOrWhiteSpace(messageId))
            throw new HubException("messageId is required.");

        var userId = GetUserId();
        var ok = await _messageService.MarkMessageReadAsync(messageId, userId);
        if (!ok)
            throw new HubException("Unable to mark message as read.");
    }

    public async Task<List<UnreadChatDto>> GetUnreadList()
    {
        var userId = GetUserId();
        return await _chatService.GetUnreadChatsAsync(userId);
    }

    public async Task JoinChat(string chatId)
    {
        EnsureChatId(chatId);
        var userId = GetUserId();

        if (!await _chatRepo.IsUserInChat(chatId, userId))
            throw new HubException("Access denied for this chat.");

        await Groups.AddToGroupAsync(Context.ConnectionId, GetChatGroupName(chatId));
    }

    public async Task LeaveChat(string chatId)
    {
        EnsureChatId(chatId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetChatGroupName(chatId));
    }

    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        var userChats = await _chatService.GetChatsForUserAsync(userId, null, null);

        foreach (var chat in userChats)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, GetChatGroupName(chat.ChatId));
        }

        await Clients.Caller.SendAsync("InitialChats", userChats);

        await base.OnConnectedAsync();
    }

    private string GetUserId()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new HubException("User id is missing from token.");

        return userId;
    }

    private static string GetChatGroupName(string chatId) => $"chat:{chatId}";

    private static void EnsureChatId(string chatId)
    {
        if (string.IsNullOrWhiteSpace(chatId))
            throw new HubException("chatId is required.");
    }
}
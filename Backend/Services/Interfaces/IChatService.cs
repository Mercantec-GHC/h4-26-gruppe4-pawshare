using Models;
using Models.DTOs;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing chat functionality including chat rooms, users, and messages.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Creates a new chat with the specified title and initial users.
    /// </summary>
    /// <param name="dto">The dto containing title and users of the chat.</param>
    /// <returns>The newly created chat.</returns>
    public Task<ChatListItemDto?> CreateChatAsync(CreateChatDto dto);

    /// <summary>
    /// Checks if a chat with the given chatId exists. Returns true if it exists, false otherwise.
    /// </summary>
    /// <param name="chatId"></param>
    /// <returns></returns>
    public Task<bool> GetChat(string chatId);

    /// <summary>
    /// Retrieves all chats that a specific user is a member of.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of chats the user belongs to.</returns>
    public Task<List<ChatListItemDto>> GetChatsForUserAsync(string userId, int? limit, int? offset);

    /// <summary>
    /// Gets all messages in given chat
    /// </summary>
    /// <param name="chatId">The Id of the chat to find messages in</param>
    /// <returns>List of messages in the chat</returns>
    public Task<List<MessageDto>> GetMessagesAsync(string chatId, int? limit, int? offset);

    /// <summary>
    /// Sends a new message to a chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="userId">The unique identifier of the user sending the message.</param>
    /// <param name="content">The content of the message.</param>
    /// <returns>boolean to reflect if it worked.</returns>
    public Task<bool> SendMessageAsync(string chatId, string userId, string content);

    /// <summary>
    /// Gets list of users unread messages
    /// </summary>
    /// <param name="userId">The id of the user</param>
    /// <returns>List of unread messages in the chat</returns>
    Task<List<UnreadChatDto>> GetUnreadChatsAsync(string userId);
}
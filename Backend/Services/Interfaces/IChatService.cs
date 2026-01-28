using Models;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing chat functionality including chat rooms, users, and messages.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Retrieves a chat by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the chat.</param>
    /// <returns>The chat with the specified ID, or null if not found.</returns>
    public Task<Chat> GetChatAsync(string id);

    /// <summary>
    /// Creates a new chat with the specified title and initial users.
    /// </summary>
    /// <param name="title">The title of the chat.</param>
    /// <param name="users">The list of users to include in the chat.</param>
    /// <returns>The newly created chat.</returns>
    public Task<Chat> CreateChatAsync(string title, List<User> users);

    /// <summary>
    /// Retrieves all chats that a specific user is a member of.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of chats the user belongs to.</returns>
    public Task<List<Chat>> GetUserChatsAsync(string userId);

    /// <summary>
    /// Adds a user to an existing chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="userId">The unique identifier of the user to add.</param>
    /// <returns>The updated chat with the new user, or null if the operation failed.</returns>
    public Task<Chat> AddUserToChatAsync(string chatId, string userId);

    /// <summary>
    /// Removes a user from an existing chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="userId">The unique identifier of the user to remove.</param>
    /// <returns>The updated chat without the removed user, or null if the operation failed.</returns>
    public Task<Chat> RemoveUserFromChatAsync(string chatId, string userId);

    /// <summary>
    /// Retrieves all messages from a specific chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <returns>A list of messages in the chat.</returns>
    public Task<List<Message>> GetChatMessagesAsync(string chatId);

    /// <summary>
    /// Sends a new message to a chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="senderId">The unique identifier of the user sending the message.</param>
    /// <param name="content">The content of the message.</param>
    /// <returns>The updated chat with the new message, or null if the operation failed.</returns>
    public Task<Chat> SendMessageAsync(string chatId, string senderId, string content);
}
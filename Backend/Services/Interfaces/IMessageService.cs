using Models;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing messages in Pawshare chats.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Gets all messages from a specific chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <returns>A list of messages in the specified chat.</returns>
    Task<List<Message>> GetMessagesByChatAsync(string chatId);

    /// <summary>
    /// Sends a new message to a chat.
    /// </summary>
    /// <param name="chatId">The unique identifier of the chat.</param>
    /// <param name="userId">The unique identifier of the user sending the message.</param>
    /// <param name="content">The content of the message.</param>
    /// <returns>The newly created message if successful, otherwise null.</returns>
    Task<Message?> SendMessageAsync(string chatId, string userId, string content);

    /// <summary>
    /// Deletes a message.
    /// </summary>
    /// <param name="id">The unique identifier of the message to delete.</param>
    /// <returns>True if the message was deleted successfully, otherwise false.</returns>
    Task<bool> DeleteMessageAsync(string id);
}

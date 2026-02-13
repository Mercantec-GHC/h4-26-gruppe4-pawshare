using Models;

namespace Services.Interfaces;

/// <summary>
/// Service interface for managing messages in Pawshare chats.
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Marks a message as read for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user reading the message.</param>
    /// <param name="messageId">The unique identifier of the message to be read.</param>
    /// <returns>True if read, false if a mistake happens.</returns>
    Task<bool> MarkMessageReadAsync(string messageId, string userId);
}

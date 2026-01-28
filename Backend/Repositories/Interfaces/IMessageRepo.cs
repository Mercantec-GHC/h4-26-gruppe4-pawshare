namespace Repositories.Interfaces;
using Models;

public interface IMessageRepo
{
    /// <summary>
    /// Gets Messages from given Chat
    /// </summary>
    /// <param name="id">The id of the chat to get messages from</param>
    /// <returns>Lists of messages from given chat or returns empty list if none is found</returns>
    public Task<List<Message>> GetMessagesFromChat(string chatId);

    /// <summary>
    /// Posts a new message to the table
    /// </summary>
    /// <param name="newMessage">The new message that needs to be posted</param>
    /// <returns>Message that was added, null if it already exists, and throws exception if error occurs under creation</returns>
    public Task<Message?> SendMessage(Message newMessage);
}
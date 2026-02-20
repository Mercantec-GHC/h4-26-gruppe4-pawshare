using Models;

namespace Repositories.Interfaces;

public interface IChatRepo
{
    /// <summary>
    /// Gets a queryable collection of Chats. (Can be used to include)
    /// </summary>
    /// <returns>An <see cref="IQueryable{Chat}"/> representing the chat table</returns>
    public IQueryable<Chat> Query();

    /// <summary>
    /// Posts a new chat to the table
    /// </summary>
    /// <param name="newChat">The new chat that needs to be posted</param>
    /// <returns>Chat that was added, null if it already exists, and throws exception if error occurs under creation</returns>
    public Task<Chat?> PostChat(Chat newChat);

    /// <summary>
    /// Gets a Chat with given Id
    /// </summary>
    /// <param name="id">The id of the wanted chat</param>
    /// <returns>Chat with given id, if not found returns   null</returns>
    public Task<Chat?> GetChat(string id);

    /// <summary>
    /// Updates given Chat
    /// </summary>
    /// <param name="chat">The new version of the Chat</param>
    /// <returns>The Chat that was updated, returns null if not successful</returns>
    public Task<Chat?> UpdateChat(Chat chat);

    /// <summary>
    /// Deletes given chat
    /// </summary>
    /// <param name="id">The id of the chat to be deleted</param>
    /// <returns>True if success and false if not</returns>
    public Task<bool> DeleteChat(string id);

    /// <summary>
    /// Checks if a user is in a given chat
    /// </summary>
    /// <param name="chatId">The id of the chat</param>
    /// <param name="userId">The id of the user to see if it is in the chat</param>
    /// <returns>True if the user is in the chat, false if not</returns>
    Task<bool> IsUserInChat(string chatId, string userId);
}
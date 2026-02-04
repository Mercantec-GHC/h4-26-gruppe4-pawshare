using Models;

namespace Repositories.Interfaces;

public interface IChatRepo
{
    /// <summary>
    /// Gets Chats with given User
    /// </summary>
    /// <param name="userId">The id of the user to get chats with</param>
    /// <returns>Lists of chats with the given user, returns empty list if none is found</returns>
    public Task<List<Chat>> GetChatsWithUser(string userId);

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
    /// <returns>Chat with given id, if not found returns null</returns>
    public Task<Chat?> GetChat(string id);

    /// <summary>
    /// Updates given Chat
    /// </summary>
    /// <param name="chat">The new version of the Chat</param>
    /// <returns>The Chat that was updated, returns null if not successful</returns>
    public Task<Chat?> UpdateChat(Chat chat);
}
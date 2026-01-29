namespace Repositories.Interfaces;
using Models;

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
}
using Models;

namespace Repositories.Interfaces;

public interface IMessageRepo
{
    /// <summary>
    /// Gets a queryable collection of Messages. (Can be used to include)
    /// </summary>
    /// <returns>An <see cref="IQueryable{Message}"/> representing the message table</returns>
    public IQueryable<Message> Query();

    /// <summary>
    /// Posts a new message to the table
    /// </summary>
    /// <param name="newMessage">The new message that needs to be posted</param>
    /// <returns>Message that was added, and throws exception if error occurs under creation</returns>
    public Task<Message?> SendMessage(Message newMessage);

    /// <summary>
    /// Updates given Message
    /// </summary>
    /// <param name="Message">The new version of the Message</param>
    /// <returns>The Message that was updated, returns null if not successful</returns>
    public Task<Message?> UpdateMessage(Message Message);

    /// <summary>
    /// Deletes Message from table
    /// </summary>
    /// <param name="MessageId">Id of the Message needed to be deleted</param>
    /// <returns>Boolean, true if succesful and false if not</returns>
    public Task<bool> DeleteMessage(string MessageId);
}
using Models;

namespace Repositories.Interfaces
{
    public interface IMessageReadReceiptRepo
    {
        /// <summary>
        /// Gets a queryable collection of ReadReceipts. (Can be used to include)
        /// </summary>
        /// <returns>An <see cref="IQueryable{Chat}"/> representing the chat table</returns>
        IQueryable<MessageReadReceipt> Query();

        /// <summary>
        /// Posts a new Read Receipt to the table
        /// </summary>
        /// <param name="newReceipt">The new read receipt that needs to be created</param>
        /// <returns>Read receipt that was added, null if it already exists, and throws exception if error occurs under creation</returns>
        Task<MessageReadReceipt?> PostReceiptAsync(MessageReadReceipt newReceipt);

        /// <summary>
        /// Checks if the message has already been read
        /// </summary>
        /// <param name="messageId">Message id to be checked if it was read</param>
        /// <param name="userId">User id to see if the message was read by the user</param>
        /// <returns>True if exists false if not</returns>
        Task<bool> ExistsAsync(string messageId, string userId);
    }
}

using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Context;
using Repositories.Interfaces;

namespace Repositories
{
    public class MessageReadReceiptRepo : IMessageReadReceiptRepo
    {
        private readonly AppDBContext _db;

        public MessageReadReceiptRepo(AppDBContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public IQueryable<MessageReadReceipt> Query()
            => _db.MessageReadReceipts.AsQueryable();

        /// <inheritdoc/>
        public async Task<MessageReadReceipt?> PostReceiptAsync(MessageReadReceipt receipt)
        {
            _db.MessageReadReceipts.Add(receipt);
            await _db.SaveChangesAsync();
            return receipt;
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(string messageId, string userId)
        {
            return await _db.MessageReadReceipts
                .AnyAsync(r => r.MessageId == messageId && r.UserId == userId);
        }
    }
}

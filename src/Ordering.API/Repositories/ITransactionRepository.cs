using Ordering.API.Model;
using Ordering.API.Models.OrderModel;

namespace Ordering.API.Repositories
{
    public interface ITransactionRepository
    {
        public Transaction AddTransaction(Transaction transaction);
        public Transaction UpdateTransaction(Transaction transaction);
        public Task<Transaction> GetTransactionByIdAsync(long transactionId);
        public Task<PaginatedItems<Transaction>> GetTransactionsFromUserAsync(Guid buyerId, int pageIndex, int pageSize);
        public Task<PaginatedItems<Transaction>> GetTransactions(int pageIndex, int pageSize);
        public void DeleteTransaction(Transaction transaction);
        public Task SaveChangesAsync();
    }
}

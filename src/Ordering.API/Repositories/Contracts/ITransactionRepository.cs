using Ordering.API.Model;
using Ordering.API.Models.OrderModel;
using Ordering.API.Models.ReportModel;

namespace Ordering.API.Repositories.Contracts
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
        public Task<List<WeeklyTransactionSummary>> GetTransactionsByWeek();
        public Task<List<MonthlyTransactionSummary>> GetTransactionsByMonth();
    }
}

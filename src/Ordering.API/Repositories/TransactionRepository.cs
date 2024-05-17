using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure;
using Ordering.API.Model;
using Ordering.API.Models.OrderModel;

namespace Ordering.API.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly OrderContext _context;

        public TransactionRepository(OrderContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Transaction AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);

            return transaction;
        }

        public void DeleteTransaction(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
        }

        public async Task<Transaction> GetTransactionByIdAsync(long transactionId)
        {
            var transaction = await _context.Transactions
                .Where(o => o.Id == transactionId)
                .Include(o => o.Buyer)
                .Include(o => o.PaymentMethod)
                .SingleOrDefaultAsync();

            return transaction;
        }

        public async Task<PaginatedItems<Transaction>> GetTransactions(int pageIndex, int pageSize)
        {
            var query = _context.Set<Transaction>().AsQueryable()
                 .OrderBy(transaction => transaction.Id);

            var totalItems = await query.LongCountAsync();

            if (pageIndex >= 0 && pageSize > 0)
            {
                query = (IOrderedQueryable<Transaction>)query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            var itemsInPage = await query.ToListAsync();

            return new PaginatedItems<Transaction>(
                pageIndex,
                pageSize,
                totalItems,
                itemsInPage);
        }

        public async Task<PaginatedItems<Transaction>> GetTransactionsFromUserAsync(Guid buyerId, int pageIndex, int pageSize)
        {
            var query = _context.Set<Transaction>().AsQueryable()
                 .OrderBy(transaction => transaction.Id)
                 .Where(transaction => transaction.BuyerId == buyerId);

            var totalItems = await query.LongCountAsync();

            if (pageIndex >= 0 && pageSize > 0)
            {
                query = (IOrderedQueryable<Transaction>)query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            var itemsInPage = await query.ToListAsync();

            return new PaginatedItems<Transaction>(
                pageIndex,
                pageSize,
                totalItems,
                itemsInPage);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Transaction UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);

            return transaction;
        }
    }
}

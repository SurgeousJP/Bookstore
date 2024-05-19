using Microsoft.EntityFrameworkCore;
using Ordering.API.Extensions;
using Ordering.API.Infrastructure;
using Ordering.API.Model;
using Ordering.API.Models;
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

        public async Task<List<WeeklyTransactionSummary>> GetTransactionsByWeek()
        {
            // Get the start date (Sunday) and end date (Saturday) of the current week
            DateTime today = DateTime.Today;
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
            DateTime startDate = today.AddDays(-daysUntilSunday);
            DateTime endDate = startDate.AddDays(6);

            var query = _context.Set<Transaction>()
                                .Where(transaction => transaction.CreatedAt >= startDate && transaction.CreatedAt <= endDate)
                                .OrderBy(transaction => transaction.CreatedAt);

            // Group transactions by day and calculate the total amount for each day
            var weeklyTransactionSummary = await query.GroupBy(
                transaction => transaction.CreatedAt.Date,
                (date, transactions) => new WeeklyTransactionSummary
                {
                    DayOfWeek = Extension.MapIntToDayOfWeek(int.Parse(date.DayOfWeek.ToString())),
                    TotalAmount = transactions.Sum(transaction => transaction.TotalAmount)
                })
                .ToListAsync();

            return weeklyTransactionSummary;
        }

        public async Task<List<MonthlyTransactionSummary>> GetTransactionsByMonth()
        {
            // Get the start date (first day) and end date (last day) of the current month
            DateTime today = DateTime.Today;
            DateTime startDate = new DateTime(today.Year, today.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var query = _context.Set<Transaction>()
                                .Where(transaction => transaction.CreatedAt >= startDate && transaction.CreatedAt <= endDate)
                                .OrderBy(transaction => transaction.CreatedAt);

            // Group transactions by day and calculate the total amount for each day
            var PeriodTransactionSummary = await query.GroupBy(
                transaction => transaction.CreatedAt.Month,
                (month, transactions) => new MonthlyTransactionSummary
                {
                    MonthOfYear = Extension.MapIntToMonth(month),
                    TotalAmount = transactions.Sum(transaction => transaction.TotalAmount)
                })
                .ToListAsync();

            return PeriodTransactionSummary;
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

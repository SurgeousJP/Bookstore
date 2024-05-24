using Microsoft.EntityFrameworkCore;
using Ordering.API.BuyerModel;
using Ordering.API.Infrastructure;
using Ordering.API.Model;
using Ordering.API.Repositories.Contracts;

namespace Ordering.API.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private OrderContext _context;

        public PaymentMethodRepository(OrderContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PaymentMethod> AddPaymentMethod(PaymentMethod paymentMethod)
        {
            await _context.PaymentMethods.AddAsync(paymentMethod);
            return paymentMethod;
        }

        public async Task DeletePaymentMethod(PaymentMethod paymentMethod)
        {
            _context.ChangeTracker.Clear(); _context.PaymentMethods.Remove(paymentMethod);
            await this.SaveChangesAsync();
        }

        public async Task<PaymentMethod> GetPaymentMethodByIdAsync(int paymentMethodId)
        {
            var paymentMethod = await _context.PaymentMethods
                .Where(b => b.Id == paymentMethodId)
                .Include(b => b.CardType)
                .SingleOrDefaultAsync();

            return paymentMethod;
        }

        public async Task<PaginatedItems<PaymentMethod>> GetPaymentMethodFromUserAsync(Guid buyerId, int pageIndex, int pageSize)
        {
            var query = _context.Set<PaymentMethod>().AsQueryable()
                 .Where(a => a.BuyerId == buyerId)
                 .Include(b => b.CardType)
                 .Skip(pageIndex * pageSize)
                 .Take(pageSize);

            var totalItems = await _context.Set<PaymentMethod>().AsQueryable()
                 .Where(a => a.BuyerId == buyerId).LongCountAsync();

            var itemsInPage = await query.ToListAsync();

            return new PaginatedItems<PaymentMethod>(
                pageIndex,
                pageSize,
                totalItems,
                itemsInPage);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PaymentMethod> UpdatePaymentMethod(PaymentMethod paymentMethod)
        {
            _context.ChangeTracker.Clear();
            _context.Update(paymentMethod);
            await this.SaveChangesAsync();
            return paymentMethod;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure;
using Ordering.API.Models.BuyerModel;

namespace Ordering.API.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private OrderContext _context;

        public BuyerRepository(OrderContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Buyer> AddAsync(Buyer buyer)
        {
            await _context.Buyers.AddAsync(buyer);
            return buyer;
        }

        public async Task<Buyer> FindAsync(Guid identityGuid)
        {
            var buyer = await _context.Buyers
                .Where(b => b.Id == identityGuid)
                .Include(b => b.PaymentMethods)
                .Include(b => b.Addresses)
                .SingleOrDefaultAsync();

            return buyer;
        }

        public Buyer Update(Buyer buyer)
        {
            _context.Buyers.Update(buyer);
            return buyer;
        }

        public async Task<IEnumerable<Cardtype>> GetAllCardTypes()
        {
            var cardtypes = await _context.Cardtypes
                .ToListAsync();
            return cardtypes;
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<long> LongCountAsync()
        {
            return await _context.Buyers.LongCountAsync();
        }
    }
}

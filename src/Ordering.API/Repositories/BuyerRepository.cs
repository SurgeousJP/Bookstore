
using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure;

namespace Ordering.API.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private OrderContext _context;

        public BuyerRepository(OrderContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Buyer Add(Buyer buyer)
        {
             _context.Buyers.Add(buyer);
            return buyer;
        }

        public async Task<Buyer> FindAsync(Guid identityGuid)
        {
            var buyer = await _context.Buyers
                .Where(b => b.Id == identityGuid)
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
    }
}

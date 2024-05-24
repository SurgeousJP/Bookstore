using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure;
using Ordering.API.Model;
using Ordering.API.Models.BuyerModel;
using Ordering.API.Repositories.Contracts;

namespace Ordering.API.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private OrderContext _context;

        public AddressRepository(OrderContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Address> AddAddress(Address address)
        {
            await _context.Addresses.AddAsync(address);
            return address;
        }

        public async Task DeleteAddress(Address address)
        {
            _context.ChangeTracker.Clear();
            _context.Addresses.Remove(address);
            await this.SaveChangesAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            var address = await _context.Addresses
                .Where(b => b.Id == addressId)
                .SingleOrDefaultAsync();

            return address;
        }

        public async Task<PaginatedItems<Address>> GetAddressesFromUserAsync(Guid buyerId, int pageIndex, int pageSize)
        {
            var query = _context.Set<Address>().AsQueryable()
                 .Where(a => a.BuyerId == buyerId)
                 .Skip(pageIndex * pageSize)
                 .Take(pageSize);

            var totalItems = await _context.Set<Address>().LongCountAsync();

            var itemsInPage = await query.ToListAsync();

            return new PaginatedItems<Address>(
                pageIndex,
                pageSize,
                totalItems,
                itemsInPage);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Address> UpdateAddress(Address address)
        {
            _context.ChangeTracker.Clear();
            _context.Update(address);
            await this.SaveChangesAsync();
            return address;
        }
    }
}

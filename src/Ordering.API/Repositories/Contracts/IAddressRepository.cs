using Ordering.API.Model;
using Ordering.API.Models.BuyerModel;

namespace Ordering.API.Repositories.Contracts
{
    public interface IAddressRepository
    {
        public Task<Address> AddAddress(Address address);
        public Task<Address> UpdateAddress(Address address);
        public Task<Address> GetAddressByIdAsync(int addressId);
        public Task<PaginatedItems<Address>> GetAddressesFromUserAsync(Guid buyerId, int pageIndex, int pageSize);
        public Task DeleteAddress(Address address);
        public Task SaveChangesAsync();
    }
}

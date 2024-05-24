using Ordering.API.Models.BuyerModel;

namespace Ordering.API.Repositories.Contracts
{
    public interface IBuyerRepository
    {
        public Task<Buyer> AddAsync(Buyer buyer);
        public Buyer Update(Buyer buyer);
        public Task<Buyer> FindAsync(Guid identityGuid);
        public Task<IEnumerable<Cardtype>> GetAllCardTypes();
        public Task SaveChangeAsync();
        public Task<long> LongCountAsync();
    }
}

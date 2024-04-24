namespace Ordering.API.Repositories
{
    public interface IBuyerRepository
    {
        public Buyer Add(Buyer buyer);
        public Buyer Update(Buyer buyer);
        public Task<Buyer> FindAsync(Guid identityGuid);
        public Task<IEnumerable<Cardtype>> GetAllCardTypes();
    }
}

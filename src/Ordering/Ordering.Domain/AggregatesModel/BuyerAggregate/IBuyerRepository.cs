namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    internal interface IBuyerRepository
    {
        Buyer Add(Buyer buyer);
        Buyer Update(Buyer buyer);
        Task<Buyer> FindAsync(string BuyerIdentityGuid);
        Task<Buyer> FindByIdAsync(string id);
    }
}

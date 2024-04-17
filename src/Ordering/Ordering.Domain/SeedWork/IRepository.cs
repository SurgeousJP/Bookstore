namespace Ordering.Domain.SeedWork
{
    internal interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}

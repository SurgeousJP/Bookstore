using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

internal interface IOrderRepository : IRepository<Order>
{
    Order Add(Order order);
    void Update(Order order);
    Task<Order> GetAsync(int orderId);
}
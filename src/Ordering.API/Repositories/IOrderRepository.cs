using Ordering.API.Model;
using Ordering.API.Models.Order;

namespace Ordering.API.Repositories
{
    public interface IOrderRepository
    {
        public Order AddOrder(Order order);
        public Order UpdateOrder(Order order);
        public Task<Order> GetOrderAsync(int orderId);
        public Task<PaginatedItems<Order>> GetOrdersFromUserAsync(Guid buyerId, int pageIndex, int pageSize);
        public Task<PaginatedItems<Order>> GetOrders(int pageIndex, int pageSize);
        public void DeleteOrder(Order order);
    }
}

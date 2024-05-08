using Ordering.API.Model;
using Ordering.API.Models.OrderModel;

namespace Ordering.API.Repositories
{
    public interface IOrderRepository
    {
        public Order AddOrder(Order order);
        public Task AddOrderItems(List<OrderItem> items);
        public Order UpdateOrder(Order order);
        public Task<Order> GetOrderByIdAsync(int orderId);
        public Task<PaginatedItems<Order>> GetOrdersFromUserAsync(Guid buyerId, int pageIndex, int pageSize);
        public Task<PaginatedItems<Order>> GetOrders(int pageIndex, int pageSize);
        public void DeleteOrder(Order order);
        public Task SaveChangesAsync();
        public Task<ICollection<OrderStatus>> GetAllOrderStatus();
    }
}

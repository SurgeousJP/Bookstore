using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure;
using Ordering.API.Model;
using Ordering.API.Models.OrderModel;

namespace Ordering.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Order AddOrder(Order order)
        {
            _context.Orders.Add(order);

            return order;
        }

        public async Task AddOrderItems(List<OrderItem> items)
        {
            _context.OrderItems.AddRange(items);
            return;
        }

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<ICollection<OrderStatus>> GetAllOrderStatus()
        {
            var orderStatuses = await _context.OrderStatuses.ToListAsync();

            return orderStatuses;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.Buyer)
                .Include(o => o.OrderItems)
                .Include(o => o.PaymentMethod)
                .Include(o => o.Address)
                .Include(o => o.OrderStatus)
                .SingleOrDefaultAsync();

            return order;
        }

        public async Task<PaginatedItems<Order>> GetOrders(int pageIndex, int pageSize)
        {
            var query = _context.Set<Order>().AsQueryable()
                 .OrderBy(order => order.Id)
                 .Include(o => o.Buyer)
                 .Include(o => o.OrderStatus)
                 .Skip(pageIndex * pageSize)
                 .Take(pageSize);

            var totalItems = await query.LongCountAsync();

            var itemsInPage = await query.ToListAsync();

            return new PaginatedItems<Order>(
                pageIndex,
                pageSize,
                totalItems,
                itemsInPage);
        }

        public async Task<PaginatedItems<Order>> GetOrdersFromUserAsync(Guid buyerId, int pageIndex, int pageSize)
        {
            var query = _context.Set<Order>().AsQueryable()
                 .OrderBy(order => order.Id)
                 .Include(o => o.Buyer)
                 .Include(o => o.OrderStatus)
                 .Skip(pageIndex * pageSize)
                 .Take(pageSize);

            var totalItems = await query.LongCountAsync();

            var itemsInPage = await query.ToListAsync();

            return new PaginatedItems<Order>(
                pageIndex,
                pageSize,
                totalItems,
                itemsInPage);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Order UpdateOrder(Order order)
        {
            _context.Orders.Update(order);

            return order;
        }
    }
}

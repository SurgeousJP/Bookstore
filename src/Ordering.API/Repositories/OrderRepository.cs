﻿using Microsoft.EntityFrameworkCore;
using Ordering.API.Infrastructure;
using Ordering.API.Model;
using Ordering.API.Models.Order;

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

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
        }

        public async Task<Order> GetOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Where(o => o.Id == orderId)
                .SingleOrDefaultAsync();

            return order;
        }

        public async Task<PaginatedItems<Order>> GetOrders(int pageIndex, int pageSize)
        {
            var query = _context.Set<Order>().AsQueryable()
                 .OrderBy(order => order.Id);

            var totalItems = await query.LongCountAsync();

            if (pageIndex >= 0 && pageSize > 0)
            {
                query = (IOrderedQueryable<Order>)query.Skip(pageIndex * pageSize).Take(pageSize);
            }

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
                 .OrderBy(order => order.OrderDate)
                 .Where(order => order.BuyerId == buyerId);

            var totalItems = await query.LongCountAsync();

            if (pageIndex >= 0 && pageSize > 0)
            {
                query = (IOrderedQueryable<Order>)query.Skip(pageIndex * pageSize).Take(pageSize);
            }

            var itemsInPage = await query.ToListAsync();

            return new PaginatedItems<Order>(
                pageIndex,
                pageSize,
                totalItems,
                itemsInPage);
        }

        public Order UpdateOrder(Order order)
        {
            _context.Orders.Update(order);

            return order;
        }
    }
}
using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events
{
    internal class OrderStatusChangedToAwaitingValidationDomainEvent : INotification
    {
        public OrderStatusChangedToAwaitingValidationDomainEvent(int orderId, IEnumerable<OrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }

        public int OrderId { get; }
        public IEnumerable<OrderItem> OrderItems { get; }
        
    }
}

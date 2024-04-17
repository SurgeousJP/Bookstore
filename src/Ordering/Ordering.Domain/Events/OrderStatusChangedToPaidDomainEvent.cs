using MediatR;

namespace Ordering.Domain.Events
{
    internal class OrderStatusChangedToPaidDomainEvent : INotification
    {
        public OrderStatusChangedToPaidDomainEvent(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }
}

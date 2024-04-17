using MediatR;

namespace Ordering.Domain.Events
{
    internal class OrderCancelledDomainEvent : INotification
    {
        public OrderCancelledDomainEvent(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }
}

using MediatR;

namespace Ordering.Domain.Events
{
    internal class OrderShippedDomainEvent : INotification
    {
        public OrderShippedDomainEvent(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId {get;}
    }
}

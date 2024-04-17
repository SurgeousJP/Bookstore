using MediatR;

namespace Ordering.Domain.Events
{
    internal class OrderStatusChangedToStockConfirmedDomainEvent : INotification
    {
        public OrderStatusChangedToStockConfirmedDomainEvent(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; }
    }
}

using EventBus.Messaging.SharedModel;

namespace EventBus.Messaging.Events
{
    public class OrderStatusChangeToPaidEvent
    {
        // Take product item out of catalog -> update inventory
        public IEnumerable<OrderItemSimplified> OrderItems { get; set; }
    }
}

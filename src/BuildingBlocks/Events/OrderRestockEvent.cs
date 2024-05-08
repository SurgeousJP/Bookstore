using EventBus.Messaging.SharedModel;

namespace EventBus.Messaging.Events
{
    public class OrderRestockEvent
    {
        // Cancel the order, if necessary, then refund it and return stock
        public IEnumerable<OrderItemSimplified> OrderItems { get; set; }
    }
}

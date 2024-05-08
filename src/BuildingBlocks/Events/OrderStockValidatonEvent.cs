using EventBus.Messaging.SharedModel;

namespace EventBus.Messaging.Events
{
    public class OrderStockValidatonEvent
    {
        public int OrderId { get; set; }
        public IEnumerable<OrderItemSimplified> OrderItems {  get; set; }
    }
}

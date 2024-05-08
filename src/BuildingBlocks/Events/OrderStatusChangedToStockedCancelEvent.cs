namespace EventBus.Messaging.Events
{
    public class OrderStatusChangedToStockedCancelEvent
    {
        // Confirm order not stocked -> update order status to OrderCancelled
        // Consumer: CatalogAPI
        public int OrderId { get; set; }
    }
}

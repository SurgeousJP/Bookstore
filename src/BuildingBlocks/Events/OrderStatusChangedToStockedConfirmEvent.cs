namespace EventBus.Messaging.Events
{
    public class OrderStatusChangedToStockedConfirmEvent
    {
        // Confirm order stocked -> update order status to OrderStockedConfirm
        public int OrderId { get; set; }
    }
}

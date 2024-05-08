namespace EventBus.Messaging.Events
{
    public class OrderStartedEvent : IntegrationEvent
    {
        // Notify basket api to delete the basket -> update order status to submitted
        // Consumer: BasketAPI
        public Guid BuyerId {  get; set; }
    }
}

namespace EventBus.Messaging.Events
{
    public class ProductPriceUpdateEvent : IntegrationEvent
    {
        public int BookId { get; set; }
        public double NewPrice { get; set; }
    }
}

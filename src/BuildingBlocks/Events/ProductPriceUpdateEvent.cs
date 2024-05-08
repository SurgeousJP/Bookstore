namespace EventBus.Messaging.Events
{
    public class ProductPriceUpdateEvent : IntegrationEvent
    {
        // update the item price in baskets which have it
        public int BookId { get; set; }
        public double NewPrice { get; set; }
    }
}

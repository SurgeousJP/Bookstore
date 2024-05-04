namespace EventBus.Messaging.Events
{
    public class BasketCheckoutEvent : IntegrationEvent
    {
        public string BuyerId { get; set; } = null!;
        public List<BasketItem> BasketItems { get; set; } = null!;
    }

    public class BasketItem
    {
        public string Id { get; set; } = null!;
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public decimal TotalUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}

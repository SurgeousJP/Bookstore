namespace EventBus.Messaging.EventBusConstant
{
    public static class EventBusConstants
    {
        public const string BasketCheckoutQueue = "basket-checkout-queue";
        public const string OrderCreatedQueue = "order-created-queue";
        public const string ExchangeName = "my-exchange";
        public const string BasketCheckoutRoutingKey = "basket.checkout";
        public const string OrderCreatedRoutingKey = "order.created";
    }
}

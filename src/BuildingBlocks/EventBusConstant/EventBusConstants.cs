namespace EventBus.Messaging.EventBusConstant
{
    public static class EventBusConstants
    {
        public const string ProductUpdateQueue = "product-update-queue";
        public const string OrderStartedQueue = "order-created-queue";
        public const string OrderStatusChangedToStockCancelQueue = "order-stock-cancel-queue";
        public const string OrderStatusChangedToStockConfirmedQueue = "order-stock-confirm-queue";
        public const string OrderStockValidationQueue = "order-validation-queue";
        public const string OrderPaidQueue = "order-paid-queue";
        public const string OrderCancelledQueue = "order-cancel-queue";
        public const string OrderRestockedQueue = "order-restock-queue";
    }
}

using EventBus.Messaging.Events;
using MassTransit;
using Ordering.API.Models.OrderModel;
using Ordering.API.Repositories;

namespace Ordering.API.EventConsumer
{
    public class OrderStatusChangedToStockedConfirmedEventConsumer : IConsumer<OrderStatusChangedToStockedConfirmEvent>
    {
        private readonly ILogger<OrderStatusChangedToStockedConfirmedEventConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderStatusChangedToStockedConfirmedEventConsumer(ILogger<OrderStatusChangedToStockedConfirmedEventConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<OrderStatusChangedToStockedConfirmEvent> context)
        {
            var orderId = context.Message.OrderId;
            _logger.LogInformation($"Began updating order status of {orderId} to StockConfirmed");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            order.OrderStatusId = OrderStatus.ORDER_STOCK_CONFIRMED;

            _orderRepository.UpdateOrder(order);

            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation($"Finished updating order status of {orderId} to StockConfirmed");
        }
    }
}

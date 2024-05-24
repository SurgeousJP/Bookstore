using EventBus.Messaging.Events;
using MassTransit;
using Ordering.API.Models.OrderModel;
using Ordering.API.Repositories.Contracts;

namespace Ordering.API.EventConsumer
{
    public class OrderStatusChangedToStockedCancelEventConsumer : IConsumer<OrderStatusChangedToStockedCancelEvent>
    {
        private readonly ILogger<OrderStatusChangedToStockedCancelEventConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderStatusChangedToStockedCancelEventConsumer(ILogger<OrderStatusChangedToStockedCancelEventConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<OrderStatusChangedToStockedCancelEvent> context)
        {
            var orderId = context.Message.OrderId;
            _logger.LogInformation($"Began updating order status of {orderId} to Cancelled  due to insufficient stock");

            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            order.OrderStatusId = OrderStatus.ORDER_STATUS_CANCELLED;

            _orderRepository.UpdateOrder(order);

            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation($"Finished updating order status of {orderId} to Cancelled");
        }
    }
}

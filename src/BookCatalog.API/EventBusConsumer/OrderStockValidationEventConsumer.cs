using BookCatalog.API.Model;
using BookCatalog.API.Repositories;
using EventBus.Messaging.Events;
using MassTransit;

namespace BookCatalog.API.EventBusConsumer
{
    public class OrderStockValidationEventConsumer : IConsumer<OrderStockValidatonEvent>
    {
        private readonly ILogger<OrderStockValidationEventConsumer> _logger;
        private readonly IRepository<Book> _bookRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderStockValidationEventConsumer(ILogger<OrderStockValidationEventConsumer> logger, IRepository<Book> bookRepository, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderStockValidatonEvent> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var book = await _bookRepository.GetItemByIdAsync(item.BookId);
                if (book.Availability < item.Quantity)
                {
                    _logger.LogInformation($"The book with book id {book.Id} is not enough to fulfill order, availability {book.Availability} < quantity {item.Quantity}");

                    _logger.LogInformation($"Began cancelling order with OrderId {context.Message.OrderId}");

                    await _publishEndpoint.Publish(new OrderStatusChangedToStockedCancelEvent
                    {
                        OrderId = context.Message.OrderId
                    });

                    return;
                }
            }

            _logger.LogInformation($"Order with OrderId {context.Message.OrderId} begin changed status to StockedConfirmed");
            await _publishEndpoint.Publish(new OrderStatusChangedToStockedConfirmEvent
            {
                OrderId = context.Message.OrderId
            });

            return;
        }
    }
}

using BookCatalog.API.Model;
using BookCatalog.API.Repositories;
using EventBus.Messaging.Events;
using MassTransit;

namespace BookCatalog.API.EventBusConsumer
{
    public class OrderRestockEventConsumer : IConsumer<OrderRestockEvent>
    {
        private readonly ILogger<OrderStockValidationEventConsumer> _logger;
        private readonly IRepository<Book> _bookRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderRestockEventConsumer(ILogger<OrderStockValidationEventConsumer> logger, IRepository<Book> bookRepository, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderRestockEvent> context)
        {
            _logger.LogInformation("Order restocking items from order...");
            foreach (var item in context.Message.OrderItems)
            {
                var book = await _bookRepository.GetItemByIdAsync(item.BookId);
                _logger.LogInformation($"The book with book id {book.Id}  availability {book.Availability}, quantity {item.Quantity}, remaining {book.Availability + item.Quantity}");

                var newAvailability = book.Availability + item.Quantity;
                book.Availability = newAvailability;

                await _bookRepository.Update(book);
            }
            await _bookRepository.SaveChangesAsync();
            _logger.LogInformation("Finished updating inventory");

            return;
        }
    }
}

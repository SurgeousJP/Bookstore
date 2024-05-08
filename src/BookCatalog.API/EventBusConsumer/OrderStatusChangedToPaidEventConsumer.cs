using BookCatalog.API.Model;
using BookCatalog.API.Repositories;
using EventBus.Messaging.Events;
using MassTransit;

namespace BookCatalog.API.EventBusConsumer
{
    public class OrderStatusChangedToPaidEventConsumer : IConsumer<OrderStatusChangeToPaidEvent>
    {
        private readonly ILogger<OrderStatusChangedToPaidEventConsumer> _logger;
        private readonly IRepository<Book> _bookRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderStatusChangedToPaidEventConsumer(ILogger<OrderStatusChangedToPaidEventConsumer> logger, IRepository<Book> bookRepository, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderStatusChangeToPaidEvent> context)
        {
            _logger.LogInformation("Order fetching items from inventory...");
            foreach (var item in context.Message.OrderItems)
            {
                var book = await _bookRepository.GetItemByIdAsync(item.BookId);
                _logger.LogInformation($"The book with book id {book.Id}  availability {book.Availability}, quantity {item.Quantity}, remaining {book.Availability - item.Quantity}");

                var newAvailability = book.Availability - item.Quantity;
                book.Availability = newAvailability;

                await _bookRepository.Update(book);
            }
            await _bookRepository.SaveChangesAsync();
            _logger.LogInformation("Finished updating inventory");

            return;
        }
    }
}

using Basket.API.Repositories;
using EventBus.Messaging.Events;
using MassTransit;

namespace Basket.API.EventConsumer
{
    public class OrderStartedEventConsumer : IConsumer<OrderStartedEvent>
    {
        private readonly ILogger<OrderStartedEventConsumer> _logger;
        private readonly IBasketRepository _basketRepository;

        public OrderStartedEventConsumer(ILogger<OrderStartedEventConsumer> logger, IBasketRepository basketRepository)
        {
            _logger = logger;
            _basketRepository = basketRepository;
        }

        public async Task Consume(ConsumeContext<OrderStartedEvent> context)
        {
            var buyerId = context.Message.BuyerId.ToString();
            await _basketRepository.DeleteBasketAsync(buyerId);
        }
    }
}

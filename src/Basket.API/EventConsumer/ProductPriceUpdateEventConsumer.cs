using EventBus.Messaging.Events;
using MassTransit;
using Basket.API.Repositories;

namespace Basket.API.EventConsumer
{
    public class ProductPriceUpdateEventConsumer : IConsumer<ProductPriceUpdateEvent>
    {
        private readonly ILogger<ProductPriceUpdateEventConsumer> logger;
        private readonly IBasketRepository basketRepository;

        public ProductPriceUpdateEventConsumer(ILogger<ProductPriceUpdateEventConsumer> logger, IBasketRepository basketRepository)
        {
            this.logger = logger;
            this.basketRepository = basketRepository;
        }

        public async Task Consume(ConsumeContext<ProductPriceUpdateEvent> context)
        {
            var message = context.Message;
            await basketRepository.UpdateItemPriceForBasket(message.BookId, message.NewPrice);
            logger.LogInformation($"ProductPriceUpdateEvent consumed successfully. Book updated Id: {message.BookId} \n New price: {message.NewPrice}");
        }
    }

}

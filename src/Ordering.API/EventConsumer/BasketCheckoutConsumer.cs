using EventBus.Messaging.Events;
using MassTransit;
using Ordering.API.Repositories;

namespace Ordering.API.EventConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly ILogger<BasketCheckoutConsumer> logger;
        private readonly IOrderRepository orderRepository;

        public BasketCheckoutConsumer(ILogger<BasketCheckoutConsumer> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            //var newOrder = await orderRepository.AddOrder()
            logger.LogInformation($"BasketCheckoutEvent consumed successfully. Created Order Id: Testing");
        }
    }

}

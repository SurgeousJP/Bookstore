using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using Basket.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using MassTransit;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger<BasketsController> logger; // Inject ILogger

        public BasketsController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint, ILogger<BasketsController> logger)
        {
            this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBasket([FromRoute] string userId)
        {
            if (userId == null ||  userId.Length == 0)
            {
                return Ok("No basket found");
            }
            var basket = await basketRepository.GetBasketAsync(userId);

            if (basket != null)
            {
                return Ok(basket);
            }
            return Ok("No basket found");
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> UpdateBasket([FromRoute] string userId, [FromBody] List<Model.BasketItem> basketItems)
        {
            var customerBasket = ModelMapper.MapToCustomerBasket(userId, basketItems);

            try
            {
                var basket = await basketRepository.UpdateBasketAsync(customerBasket);
                return Ok(new {
                    Message = "Basket updated successfully",
                    Basket = basket
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteBasket(string userId)
        {
            await basketRepository.DeleteBasketAsync(userId);
            return Ok("Basket deleted successfully");
        }

        [HttpPost("checkout/{userId}")]
        public async Task<IActionResult> Checkout([FromRoute] string userId)
        {
            var basket = await basketRepository.GetBasketAsync(userId);

            if (basket == null)
            {
                return BadRequest("Customer basket not found !");
            }

            var eventMessage = ModelMapper.MapToBasketCheckoutEvent(userId, basket.Items);

            await publishEndpoint.Publish(eventMessage);

            await basketRepository.DeleteBasketAsync(userId);

            return Accepted("Order is being created");
        }
    }
}

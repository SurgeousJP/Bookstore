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

        public BasketsController(IBasketRepository basketRepository, IPublishEndpoint publishEndpoint)
        {
            this.basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
            this.publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetBasket([FromRoute] string userId)
        {
            if (userId == null ||  userId.Length == 0)
            {
                return Ok("User id is null or empty");
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
    }
}

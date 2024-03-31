using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Basket.API.Model;
using Basket.API.gRPC;
using Basket.API.Repositories;
using Basket.API.Extensions;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        [HttpGet("get/{userId}")]
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

        [HttpPost("update/{userId}")]
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

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteBasket(string userId)
        {
            await basketRepository.DeleteBasketAsync(userId);
            return Ok("Basket deleted successfully");
        }
    }
}

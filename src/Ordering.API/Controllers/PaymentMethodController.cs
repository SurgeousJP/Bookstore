using Microsoft.AspNetCore.Mvc;
using Ordering.API.BuyerModel;
using Ordering.API.Model;
using Ordering.API.Models;
using Ordering.API.Models.DTOs;
using Ordering.API.Repositories.Contracts;

namespace Ordering.API.Controllers
{
    [Route("api/v1/payment-methods")]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;
        private readonly IBuyerRepository _buyerRepository;
        private readonly ILogger<PaymentMethodController> _logger;

        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository, IBuyerRepository buyerRepository, ILogger<PaymentMethodController> logger)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _buyerRepository = buyerRepository;
            _logger = logger;
        }

        [HttpPost("{buyerId}")]
        public async Task<IActionResult> CreatePaymentMethod([FromRoute] Guid buyerId, [FromBody] PaymentMethodDTO addPaymentMethod)
        {
            var buyer = await _buyerRepository.FindAsync(buyerId);

            if (buyer == null)
            {
                _logger.LogInformation("The buyer is null");
                return BadRequest("Buyer does not exist");
            }


            _logger.LogInformation("Begin create payment method");
            var buyerPaymentMethod = buyer.VerifyOrAddPaymentMethod(addPaymentMethod);
            await _buyerRepository.SaveChangeAsync();

            return Ok(buyerPaymentMethod);
        }

        [HttpGet("{buyerId}")]
        public async Task<IActionResult> GetPaymentMethodForBuyer(
            [FromRoute] Guid buyerId,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10
            )
        {
            var itemsOnPageQuery = await _paymentMethodRepository.GetPaymentMethodFromUserAsync(buyerId, pageIndex, pageSize);

            if (itemsOnPageQuery.Data == null || itemsOnPageQuery.Data.Count() == 0)
            {
                return NotFound("Payment methods not found");
            }
            else
            {
                return Ok(new PaginatedItems<PaymentMethodDTO>(
                    itemsOnPageQuery.PageIndex,
                    itemsOnPageQuery.PageSize,
                    itemsOnPageQuery.TotalItems,
                    itemsOnPageQuery.Data.Select(i => OrderMapper.ToPaymentMethodDTO(i)).ToList()));
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePaymentMethod([FromRoute] int id, [FromBody] PaymentMethodDTO updatePaymentMethod)
        {
            var existingMethod = await _paymentMethodRepository.GetPaymentMethodByIdAsync(id);

            if (existingMethod == null)
            {
                return NotFound("Payment method not found for update");
            }

            OrderMapper.MapPaymentMethod(OrderMapper.ToPaymentMethod(updatePaymentMethod), existingMethod);
            await _paymentMethodRepository.UpdatePaymentMethod(existingMethod);

            return Ok("Payment method updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod([FromRoute] int id)
        {
            var existingMethod = _paymentMethodRepository.GetPaymentMethodByIdAsync(id);

            if (existingMethod == null)
            {
                return NotFound("Payment method not found for deletion");
            }

            await _paymentMethodRepository.DeletePaymentMethod(new PaymentMethod { Id = id });

            return Ok("Payment method deleted successfully");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Ordering.API.Model;
using Ordering.API.Models;
using Ordering.API.Models.BuyerModel;
using Ordering.API.Models.DTOs;
using Ordering.API.Repositories.Contracts;

namespace Ordering.API.Controllers
{
    [Route("api/v1/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IBuyerRepository _buyerRepository;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressRepository addressRepository, IBuyerRepository buyerRepository, ILogger<AddressController> logger)
        {
            _addressRepository = addressRepository;
            _buyerRepository = buyerRepository;
            _logger = logger;
        }

        [HttpPost("{buyerId}")]
        public async Task<IActionResult> CreateAddress([FromRoute] Guid buyerId, [FromBody] AddressDTO addAddress)
        {
            var buyer = await _buyerRepository.FindAsync(buyerId);

            if (buyer == null)
            {
                _logger.LogInformation("The buyer is null");
                return BadRequest("Buyer does not exist");
            }


            _logger.LogInformation("Begin create address");
            var buyerAddress = buyer.VerifyOrAddAddress(addAddress);
            await _buyerRepository.SaveChangeAsync();

            return Ok(buyerAddress);
        }

        [HttpGet("{buyerId}")]
        public async Task<IActionResult> GetAddressForBuyer(
            [FromRoute] Guid buyerId,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 10
            )
        {
            var itemsOnPageQuery = await _addressRepository.GetAddressesFromUserAsync(buyerId, pageIndex, pageSize);

            if (itemsOnPageQuery.Data == null || itemsOnPageQuery.Data.Count() == 0)
            {
                return NotFound("Payment methods not found");
            }
            else
            {
                return Ok(new PaginatedItems<AddressDTO>(
                    itemsOnPageQuery.PageIndex,
                    itemsOnPageQuery.PageSize,
                    itemsOnPageQuery.TotalItems,
                    itemsOnPageQuery.Data.Select(i => OrderMapper.ToAddressDTO(i)).ToList()));
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] int id, [FromBody] AddressDTO updateAddress)
        {
            var existingMethod = await _addressRepository.GetAddressByIdAsync(id);

            if (existingMethod == null)
            {
                return NotFound("Address not found for update");
            }

            await _addressRepository.UpdateAddress(OrderMapper.ToAddress(updateAddress));

            return Ok("Address updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] int id)
        {
            var existingMethod = _addressRepository.GetAddressByIdAsync(id);

            if (existingMethod == null)
            {
                return NotFound("Address not found for deletion");
            }

            await _addressRepository.DeleteAddress(new Address { Id = id });

            return Ok("deleted successfully");
        }
    }
}

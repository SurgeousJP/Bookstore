using BookCatalog.API.Queries.Mappers;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Model;
using Ordering.API.Models.DTOs;
using Ordering.API.Repositories;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderController(IBuyerRepository buyerRepository, IOrderRepository orderRepository)
        {
            _buyerRepository = buyerRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("/order/{id}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] int orderId)
        {
            var order = await _orderRepository.GetOrderAsync(orderId);

            if (order == null)
            {
                return NotFound("Order not found");
            }
            
            return Ok(OrderMapper.ToOrderDetailDTO(order));
        }
        [HttpGet("/orders/{userId}")]
        public async Task<IActionResult> GetOrdersFromBuyerAsync([FromRoute] Guid buyerId, [FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderRepository.GetOrdersFromUserAsync(buyerId, pageIndex, pageSize);

            if (orders == null || orders.TotalItems == 0)
            {
                return NotFound("Orders not found");
            }

            return Ok(new PaginatedItems<OrderDTO>(
                orders.PageIndex,
                orders.PageSize,
                orders.TotalItems,
                orders.Data.Select(
                    order => OrderMapper.ToOrderDTO(order))
                .ToList()));
        }

        [HttpGet("/orders")]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderRepository.GetOrders(pageIndex, pageSize);

            if (orders == null || orders.TotalItems == 0)
            {
                return NotFound("Orders not found");
            }

            return Ok(new PaginatedItems<OrderDTO>(
                orders.PageIndex,
                orders.PageSize,
                orders.TotalItems,
                orders.Data.Select(
                    order => OrderMapper.ToOrderDTO(order))
                .ToList()));
        }

        [HttpGet("/cardtypes")]
        public async Task<IActionResult> GetCardTypesAsync()
        {
            var cardTypes = await _buyerRepository.GetAllCardTypes();

            if (cardTypes == null)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(cardTypes);
        }
        //[HttpPost("/create-order")]
        //public Task<IActionResult> CreateOrderFromBasket();

        // public Task<IActionResult> CheckStockForOrder();

        //[HttpPatch("/ship-order")]
        //public Task<IActionResult> ShipOrderAsync();

        [HttpPatch("/cancel-order")]
        public async Task<IActionResult> CancelOrderAsync([FromQuery] int orderId, [FromQuery] Guid buyerId)
        {
            var orderForUpdate = await _orderRepository.GetOrdersFromUserAsync(buyerId, 0, 1);

            if (orderForUpdate.Data == null || orderForUpdate.Data.Count == 0)
            {
                return NotFound("Order not found for cancel");
            }

            orderForUpdate.Data[0].
        }
    }
}

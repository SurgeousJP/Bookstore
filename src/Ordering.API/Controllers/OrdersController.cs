using EventBus.Messaging.Events;
using EventBus.Messaging.SharedModel;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Model;
using Ordering.API.Models;
using Ordering.API.Models.BuyerModel;
using Ordering.API.Models.DTOs;
using Ordering.API.Models.OrderModel;
using Ordering.API.Repositories.Contracts;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IBuyerRepository _buyerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<OrdersController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(IBuyerRepository buyerRepository, IOrderRepository orderRepository, ILogger<OrdersController> logger, IPublishEndpoint publishEndpoint, ITransactionRepository transactionRepository)
        {
            _buyerRepository = buyerRepository;
            _orderRepository = orderRepository;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _transactionRepository = transactionRepository;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            return Ok(OrderMapper.ToOrderDetailDTO(order));
        }
        [HttpGet("buyer/{buyerId}")]
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

        [HttpGet("")]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var orders = await _orderRepository.GetOrders(pageIndex, pageSize);

            if (orders.Data == null || orders.Data.Count == 0)
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

        [AllowAnonymous]
        [HttpGet("cardtypes")]
        public async Task<IActionResult> GetCardTypesAsync()
        {
            var cardTypes = await _buyerRepository.GetAllCardTypes();

            if (cardTypes == null)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(cardTypes);
        }

        [AllowAnonymous]
        [HttpGet("all-status")]
        public async Task<IActionResult> GetAllOrderStatus()
        {
            var orderStatus = await _orderRepository.GetAllOrderStatus();

            if (orderStatus == null)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(orderStatus);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReportMetrics()
        {
            var totalBuyers = await _buyerRepository.LongCountAsync();
            var orders = await _orderRepository.GetOrders(0, 10);
            var totalOrders = orders.TotalItems;
            var transactions = await _transactionRepository.GetTransactions(0, 10);
            var totalRevenue = transactions.Data.Sum(transaction => transaction.TotalAmount);

            return Ok(new
            {
                CustomerCount = totalBuyers,
                OrderCount = totalOrders,
                TotalRevenue = totalRevenue,
            });
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateOrderFromBasket([FromBody] CreateOrderDTO createOrderDTO)
        {
            // buyerid -> verify if not exists -> create
            // address if not exists -> create else get and put fk
            // paymentmethod -> verify info if not exists -> create else put fk
            // items 

            var buyer = await _buyerRepository.FindAsync(createOrderDTO.userId);

            if (buyer == null)
            {
                _logger.LogInformation("The buyer is null, begin create new buyer");
                await _buyerRepository.AddAsync(new Buyer
                {
                    Id = createOrderDTO.userId,
                    Name = createOrderDTO.userName,
                });

                await _buyerRepository.SaveChangeAsync();

                buyer = await _buyerRepository.FindAsync(createOrderDTO.userId);
            }

            _logger.LogInformation("Begin fetch payment method and address");
            var buyerPaymentMethod = buyer.VerifyOrAddPaymentMethod(createOrderDTO.paymentMethod);
            var buyerOrderAddress = buyer.VerifyOrAddAddress(createOrderDTO.address);

            await _buyerRepository.SaveChangeAsync();

            _logger.LogInformation("Begin create new order");
            var order = new Order
            {
                AddressId = buyerOrderAddress.Id,
                BuyerId = buyer.Id,
                OrderStatusId = OrderStatus.ORDER_SUBMITTED,
                Description = createOrderDTO.description,
                PaymentMethodId = buyerPaymentMethod.Id,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                TotalAmount = createOrderDTO.orderItems.Sum(item => item.UnitPrice * item.Quantity),
            };

            _orderRepository.AddOrder(order);
            await _orderRepository.SaveChangesAsync();

            foreach (var orderItem in createOrderDTO.orderItems)
            {
                orderItem.OrderId = order.Id;
            }

            await _orderRepository.AddOrderItems(createOrderDTO.orderItems);
            await _orderRepository.SaveChangesAsync();

            _logger.LogInformation($"The order with {order.Id} has been created successfully");

            _logger.LogInformation($"Began publishing OrderStartedEvent with order id {order.Id}");
            await _publishEndpoint.Publish(new OrderStartedEvent
            {
                BuyerId = buyer.Id
            });

            _logger.LogInformation($"Began OrderStockValidatonEvent with order id {order.Id}");
            await _publishEndpoint.Publish(new OrderStockValidatonEvent
            {
                OrderId = (int)order.Id,
                OrderItems = createOrderDTO.orderItems.Select(i => new OrderItemSimplified { BookId = (int)i.BookId, Quantity = (int)i.Quantity })
            });

            return Ok(OrderMapper.ToOrderDetailDTO(order));
        }

        [HttpPatch("{orderId}/status/{orderStatusId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync([FromRoute] int orderId, [FromRoute] int orderStatusId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order == null)
            {
                return BadRequest("Order not found, please try again");
            }

            order.OrderStatusId = orderStatusId;

            _orderRepository.UpdateOrder(order);

            return Ok("Order status updated successfully");
        }

        [HttpPatch("{orderId}/ship")]
        public async Task<IActionResult> ShipOrderAsync([FromRoute] int orderId)
        {
            await UpdateOrderStatusAsync(orderId, OrderStatus.ORDER_SHIPPED);
            return Ok("Order status changed to shipped");
        }

        [HttpPatch("{orderId}/paid")]
        public async Task<IActionResult> PayOrderAsync([FromRoute] int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            _logger.LogInformation($"Began publishing OrderStatusChangeToPaidEvent with order id {order.Id}");
            await _publishEndpoint.Publish(new OrderStatusChangeToPaidEvent
            {
                OrderItems = order.OrderItems.Select(i => new OrderItemSimplified { BookId = (int)i.BookId, Quantity = (int)i.Quantity })
            });

            _logger.LogInformation($"Finished OrderStatusChangeToPaidEvent with order id {order.Id}");

            await UpdateOrderStatusAsync(orderId, OrderStatus.ORDER_PAID);
            return Ok("Order status changed to paid");
        }

        [HttpPatch("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrderAsync([FromRoute] int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            if (order.OrderStatusId != OrderStatus.ORDER_SUBMITTED || order.OrderStatusId != OrderStatus.ORDER_STATUS_CANCELLED)
            {
                _logger.LogInformation($"Began publishing OrderRestockEvent with order id {order.Id}");
                await _publishEndpoint.Publish(new OrderRestockEvent
                {
                    OrderItems = order.OrderItems.Select(i => new OrderItemSimplified { BookId = (int)i.BookId, Quantity = (int)i.Quantity })
                });
                _logger.LogInformation($"Finished publishing OrderRestockEvent with order id {order.Id}");
            }

            if (order.OrderStatusId == OrderStatus.ORDER_SHIPPED || order.OrderStatusId == OrderStatus.ORDER_PAID)
            {
                // Refund
            }

            await UpdateOrderStatusAsync(orderId, OrderStatus.ORDER_STATUS_CANCELLED);
            return Ok("Order status changed to cancelled");
        }

        [HttpGet("top-10-products")]
        public async Task<IActionResult> GetTopTenProducts()
        {
            var topProducts = await _orderRepository.GetTopTenProducts();

            if (topProducts == null || topProducts.Count == 0)
            {
                return BadRequest("There are no top products available");
            }

            return Ok(topProducts);
        }
    }
}

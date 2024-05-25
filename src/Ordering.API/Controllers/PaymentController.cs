using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ordering.API.Extensions;
using Ordering.API.Models.DTOs;
using Stripe;
using Stripe.Checkout;

namespace Ordering.API.Controllers
{
    [Route("api/v1/payment-webhook")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly StripeSettings _stripeAssessor;

        public PaymentController(ILogger<PaymentController> logger, IOptions<StripeSettings> assessor)
        {
            this._logger = logger;
            this._stripeAssessor = assessor.Value;
        }

        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession(List<OrderItemDTO> orderItems)
        {
            _logger.LogInformation("Created checkout session");

            var lineItems = orderItems.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long?)(item.UnitPrice * 100), // Convert to cents
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Title,
                        Metadata = new Dictionary<string, string>
                {
                    { "BookId", item.BookId.ToString() },
                    { "OldUnitPrice", item.OldUnitPrice?.ToString() },
                    { "TotalUnitPrice", item.TotalUnitPrice?.ToString() },
                    { "ImageUrl", item.ImageUrl }
                }
                    }
                },
                Quantity = item.Quantity
            }).ToList();

            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "http://localhost:3000/payment-success",
                CancelUrl = "http://localhost:3000/payment-cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return Ok(session.Url);
        }


        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeAssessor.WebhookSecretKey
                );

                _logger.LogInformation($"Received Stripe event: {stripeEvent}");
                _logger.LogInformation($"Event type: {stripeEvent.Type}");

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var options = new SessionGetOptions();
                    options.AddExpand("line_items");

                    var service = new SessionService();
                    Session sessionWithLineItems = service.Get(session.Id, options);
                    StripeList<LineItem> lineItems = sessionWithLineItems.LineItems;

                    // Convert Stripe line items to OrderItemDTO
                    var orderItems = lineItems.Data.Select(lineItem => new OrderItemDTO
                    {
                        BookId = long.Parse(lineItem.Price.Product.Metadata["BookId"]),
                        Title = lineItem.Description,
                        UnitPrice = (float)lineItem.Price.UnitAmount / 100, // Convert from cents to dollars
                        OldUnitPrice = lineItem.Price.Product.Metadata.ContainsKey("OldUnitPrice")
                                       ? float.Parse(lineItem.Price.Product.Metadata["OldUnitPrice"])
                                       : (float?)null,
                        TotalUnitPrice = lineItem.Price.Product.Metadata.ContainsKey("TotalUnitPrice")
                                         ? float.Parse(lineItem.Price.Product.Metadata["TotalUnitPrice"])
                                         : (float?)null,
                        Quantity = (int?)lineItem.Quantity,
                        ImageUrl = lineItem.Price.Product.Metadata.ContainsKey("ImageUrl")
                                   ? lineItem.Price.Product.Metadata["ImageUrl"]
                                   : null
                    }).ToList();

                    // Fulfill the purchase...
                    FulfillOrder(orderItems);
                }

                return Ok("Payment has been completed");
            }
            catch (StripeException e)
            {
                _logger.LogError("Stripe error: " + e.Message);
                return BadRequest("Error message: " + e.Message);
            }
        }

        private void FulfillOrder(List<OrderItemDTO> orderItems)
        {
            foreach (var item in orderItems)
            {
                _logger.LogInformation($"Processing order item: {item.Title}, Quantity: {item.Quantity}");
                // Additional processing logic here
            }
            _logger.LogInformation("The order has been paid, awaiting shipment");
        }
    }
}

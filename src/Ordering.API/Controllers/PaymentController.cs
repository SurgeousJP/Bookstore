using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ordering.API.Extensions;
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
        public async Task<IActionResult> CreateCheckoutSession()
        {
            _logger.LogInformation("Created checkout session");
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 200,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "T-shirt"
                            },
                        },
                        Quantity = 1
                    },
                },
                Mode = "payment",
                SuccessUrl = "http://localhost:8080/success",
                CancelUrl = "http://localhost:8080/cancel",
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
                  _stripeAssessor.StripeAPIKey
                );

                _logger.LogInformation($"{stripeEvent}");
                _logger.LogInformation($"Event type: {stripeEvent.Type}");
                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    var options = new SessionGetOptions();
                    options.AddExpand("line_items");

                    var service = new SessionService();
                    // Retrieve the session. If you require line items in the response, you may include them by expanding line_items.
                    Session sessionWithLineItems = service.Get(session.Id, options);
                    StripeList<LineItem> lineItems = sessionWithLineItems.LineItems;

                    // Fulfill the purchase...
                    this.FulfillOrder(lineItems);
                }

                return Ok("Payment has been completed");
            }
            catch (StripeException e)
            {
                _logger.LogInformation("An error has been occurred");
                return BadRequest("Error message: " + e.Message);
            }
        }

        private void FulfillOrder(StripeList<LineItem> lineItems)
        {
            // TODO: fill me in
            _logger.LogInformation("The order has been paid, awaiting shipment");
        }
    }
}

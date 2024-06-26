using EventApp.Core.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Text.Json;

namespace EventApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;

        public PaymentController(ILogger<EventsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(int eventId, EventDto eventDto)
        {
            try
            {
                // Retrieve event details based on eventId (from your database)
                // Calculate the total amount (event.TicketPrice) in cents

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(eventDto.TicketPrice * 100), // Convert to cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = eventDto.EventName,
                                Description = "Event ticket"
                            }
                        },
                        Quantity = 1
                    }
                },
                    Mode = "payment",
                    SuccessUrl = "https://yourwebsite.com/success", // Redirect after successful payment
                    CancelUrl = "https://yourwebsite.com/cancel" // Redirect if user cancels
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);

                return new JsonResult(new { sessionId = session.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating checkout session.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}

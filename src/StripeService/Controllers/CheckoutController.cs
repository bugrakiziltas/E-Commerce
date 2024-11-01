using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShoppingCartService.Models.RequestModels;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using StripeService.Data;
using StripeService.Models;

namespace StripeService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : ControllerBase
    {
        private readonly StripeSettings stripeSettings;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly StripeDbContext _context;
        public CheckoutController(IOptions<StripeSettings> stripeSettings, IPublishEndpoint publishEndpoint, StripeDbContext context)
        {
            this.stripeSettings=stripeSettings.Value;
            _publishEndpoint=publishEndpoint;
            _context=context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession(orderRequest request)
        {
            var lineItems=new List<SessionLineItemOptions>();
            foreach(var item in request.products){
                lineItems.Add(new SessionLineItemOptions{   
                    PriceData=new SessionLineItemPriceDataOptions
                    {
                        Currency="USD",
                        ProductData=new SessionLineItemPriceDataProductDataOptions
                        {
                            Name=item.Name
                        },
                        UnitAmount=(long)item.Price
                    },
                    Quantity=1
                });
            }
            var options=new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes=new List<string>{"card"},
                LineItems=lineItems,
                Mode="payment",
                SuccessUrl = "https://example.com/success", // Default success URL
                CancelUrl = "https://example.com/cancel",   // Default cancel URL
                Metadata=new Dictionary<string, string>
                {
                    {"UserId",request.userId},
                    {"Email",request.email},
                    {"Username",request.username},
                    {"Products",JsonSerializer.Serialize(request.products.Select(x=>x.Id).ToList())}
                }
            };
            var service=new Stripe.Checkout.SessionService();
            var session=await service.CreateAsync(options);
            return Ok(new {sessionId=session.Id, checkoutUrl=session.Url});
        }
        
    [HttpPost]
    [Route("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                stripeSettings.WHSecret,
                throwOnApiVersionMismatch:false
            );
            if (stripeEvent.Type=="checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                await HandleCompletedCheckoutSession(session);
            }
            else
            {
                Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
            }              
            return Ok();
        }
        catch (StripeException e)
        {
            var error=e.StripeError.Message;
            Console.WriteLine(e.StripeError.Message);
            return BadRequest();
        }
    }

    private async Task HandleCompletedCheckoutSession(Stripe.Checkout.Session session)
    {
        var service = new PaymentIntentService();
        var paymentIntent = await service.GetAsync(session.PaymentIntentId);

        if (paymentIntent.Status == "succeeded")
        {
            var productIdsJson=session.Metadata["Products"];
            var order=new OrderCreated{
                userId=session.Metadata["UserId"],
                email=session.Metadata["Email"],
                username=session.Metadata["Username"],
                productIds=JsonSerializer.Deserialize<List<int>>(productIdsJson)
            };
            await _publishEndpoint.Publish(order);
            await _context.SaveChangesAsync();
        }
    }
    }
}
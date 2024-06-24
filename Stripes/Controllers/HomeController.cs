using Microsoft.AspNetCore.Mvc;
using Stripes.Models;
using System.Diagnostics;
using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Options;

namespace Stripes.Controllers
{
	public class HomeController : Controller
	{
		private readonly StripeSettings _SstripeSettings;
		public HomeController(IOptions<StripeSettings> settings)
		{
			_SstripeSettings = settings.Value;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CreateCheckOutSession()
		{
			string amount = "25";
			var currency = "EGP";
			var SuccessURL = "https://parkpoint.netlify.app/";
			var cancelURL = "https://parkpoint.netlify.app/";
			StripeConfiguration.ApiKey = _SstripeSettings.SecretKey;

			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string>
				{
					"card"
				},
				LineItems = new List<SessionLineItemOptions>
				{
					new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							Currency = currency,
							UnitAmount = Convert.ToInt32(amount)*100,
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = "Your Payment Data",
								Description = "The Required Price"
							}
						},
						Quantity = 1,
					}
				},
				Mode = "payment",
				SuccessUrl = SuccessURL,
				CancelUrl = cancelURL,
			};
			var service = new SessionService();
			var session = service.Create(options);

			return Redirect(session.Url);
		}
		public async Task<IActionResult> success()
		{
			return View("Index");
		}
		public async Task<IActionResult> cancel()
		{
			return View("Index");
		}
		[ResponseCache(Duration = 0, Location =ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id??HttpContext.TraceIdentifier});
		}
	}
}
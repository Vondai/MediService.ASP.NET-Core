using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Services.Subscriptions;
using MediService.ASP.NET_Core.Models.Subscriptions;

using static MediService.ASP.NET_Core.WebConstants.GlobalMessage;

namespace MediService.ASP.NET_Core.Areas.Admin.Controllers
{
    public class SubscriptionsController : AdminController
    {
        private readonly ISubscriptionService subscriptions;

        public SubscriptionsController(ISubscriptionService subscriptions)
            => this.subscriptions = subscriptions;

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(SubscriptionAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var parsedPrice = decimal.TryParse(model.Price, out var price);
            if (!parsedPrice)
            {
                ModelState.AddModelError(nameof(model.Price), "Invalid Price.");
                return View(model);
            }

            await this.subscriptions.CreateSubscription(model.Name, price, model.AppointmentCount);

            TempData.Add(SuccessKey, "Successfuly added subscription plan.");
            return Redirect("/Subscriptions/All");
        }
    }
}

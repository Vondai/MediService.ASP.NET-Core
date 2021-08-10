using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Models.Subscriptions;
using MediService.ASP.NET_Core.Services.Subscriptions;

using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.ASP.NET_Core.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionService subscriptions;
        private readonly IMemoryCache cache;
        private readonly UserManager<User> userManager;

        public SubscriptionsController(ISubscriptionService subscriptions, IMemoryCache cache, UserManager<User> userManager)
        {
            this.subscriptions = subscriptions;
            this.cache = cache;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult All()
        {
            var subs = this.cache.Get<ICollection<SubscriptionViewModel>>(AllSubscriptionsKey);
            if (subs == null)
            {
                subs = this.subscriptions.GetAll();

                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(1));
                this.cache.Set(AllSubscriptionsKey, subs, options);
            }

            return View(subs);
        }

        [Authorize]
        public IActionResult Subscribe()
        {
            var userId = this.User.Id();
            var activeAppointments = this.subscriptions.ActiveAppointments(userId);
            if (activeAppointments > 0)
            {
                TempData.Add("Error", "You have active appointments.");
                return Redirect("/Appointments/Mine");
            }
            return View(new SubscribeFormModel()
            {
                Subscriptions = this.subscriptions.GetSubscriptions()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Subscribe(SubscribeFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new SubscribeFormModel() { Subscriptions = this.subscriptions.GetSubscriptions() });
            }
            var subscription = this.subscriptions.GetSubscription(model.SubscriptionId);
            if (subscription == null)
            {
                ModelState.AddModelError(nameof(model.SubscriptionId), "Select a valid subscription plan.");
                model.Subscriptions = this.subscriptions.GetSubscriptions();
                return View(model);
            }
            var user = await this.userManager.GetUserAsync(this.User);
            await this.subscriptions.SubscribeUser(subscription, user);
            TempData.Add("Success", "Successful subscription.");

            return Redirect("/Appointments/Make");
        }
    }
}

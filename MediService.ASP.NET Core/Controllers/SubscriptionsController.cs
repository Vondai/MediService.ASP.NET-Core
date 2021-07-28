using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Models.Subscriptions;

namespace MediService.ASP.NET_Core.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly MediServiceDbContext data;

        public SubscriptionsController(MediServiceDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            var subscriptions = this.data.Subscriptions
                .OrderBy(x => x.Price)
                .Select(s => new SubscriptionViewModel()
                {
                    Name = s.Name,
                    Price = s.Price.ToString(),
                    CountServices = s.CountService.ToString(),
                })
                .ToList();

            return View(subscriptions);
        }

        public IActionResult Subscribe()
        {
            var userId = this.User.Id();
            var isSubscriber = this.data
                .Users
                .Any(u => u.Id == userId && u.SubscriptionId.HasValue);
            if (isSubscriber)
            {
                return Redirect("/Appointments/Make");
            }

            return View(new SubscribeFormModel()
            {
                Subscriptions = GetSubscriptions()
            });
        }

        [HttpPost]
        public IActionResult Subscribe(SubscribeFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new SubscribeFormModel() { Subscriptions = GetSubscriptions() });
            }
            var subscription = this.data
                .Subscriptions
                .FirstOrDefault(x => x.Id == model.SubscriptionId);
            if (subscription == null)
            {
                ModelState.AddModelError(nameof(model.SubscriptionId), "Select a subscription plan.");
                model.Subscriptions = GetSubscriptions();
                return View(model);
            }
            var user = this.data
                .Users.FirstOrDefault(x => x.Id == this.User.Id());
            subscription.Users.Add(user);
            this.data.SaveChanges();
            return Redirect("/Appointments/Make");
        }

        public Dictionary<int, SubscriptionFormModel> GetSubscriptions()
        => this.data
            .Subscriptions
            .OrderBy(s => s.Price)
            .Select(x => new SubscriptionFormModel()
            {
                Id = x.Id,
                CountService = x.CountService,
                Name = x.Name,
                Price = x.Price.ToString(),
            })
            .ToDictionary(x => x.Id);

    }
}

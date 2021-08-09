using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public IActionResult All()
        {
            var subscriptions = this.data.Subscriptions
                .OrderBy(x => x.Price)
                .Select(s => new SubscriptionViewModel()
                {
                    Name = s.Name,
                    Price = s.Price.ToString(),
                    AppointmentCount = s.AppointmentCount.ToString(),
                })
                .ToList();

            return View(subscriptions);
        }

        [Authorize]
        public IActionResult Subscribe()
        {
            var userId = this.User.Id();
            var activeAppointments = this.data
                .Appointments
                .Where(a => a.User.Id == userId && a.IsCanceled == false && a.IsDone == false)
                .Count();
            if (activeAppointments > 0)
            {
                TempData.Add("Error", "You have active appointments.");
                return Redirect("/Appointments/Mine");
            }
            return View(new SubscribeFormModel()
            {
                Subscriptions = GetSubscriptions()
            });
        }

        [HttpPost]
        [Authorize]
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
            var currentSubscription = this.data
                .Subscriptions
                .Where(x => x.Users.Contains(user))
                .FirstOrDefault();
            if (currentSubscription != null)
            {
                currentSubscription.Users.Remove(user);
            }
            subscription.Users.Add(user);
            this.data.SaveChanges();
            TempData.Add("Success", "Successful subscription.");
            return Redirect("/Appointments/Make");
        }

        public Dictionary<int, SubscriptionFormModel> GetSubscriptions()
        => this.data
            .Subscriptions
            .OrderBy(s => s.Price)
            .Select(x => new SubscriptionFormModel()
            {
                Id = x.Id,
                AppointmentCount = x.AppointmentCount,
                Name = x.Name,
                Price = x.Price.ToString(),
            })
            .ToDictionary(x => x.Id);

    }
}

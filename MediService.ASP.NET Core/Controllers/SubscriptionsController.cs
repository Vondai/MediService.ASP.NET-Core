using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Models.Subscriptions;
using MediService.ASP.NET_Core.Services.Appointments;
using MediService.ASP.NET_Core.Services.Specialists;
using MediService.ASP.NET_Core.Services.Subscriptions;

using static MediService.ASP.NET_Core.Areas.Admin.AdminConstants;

using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.ASP.NET_Core.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly ISubscriptionService subscriptions;
        private readonly IAppointmentService appointments;
        private readonly ISpecialistService specialists;
        private readonly IMemoryCache cache;

        public SubscriptionsController
            (ISubscriptionService subscriptions,
            IMemoryCache cache,
            IAppointmentService appointments,
            ISpecialistService specialists)
        {
            this.subscriptions = subscriptions;
            this.cache = cache;
            this.appointments = appointments;
            this.specialists = specialists;
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
            var isAdmin = this.User.IsInRole(AdminRoleName);
            if (isAdmin)
            {
                TempData.Add("Error", "Admins cannot subscribe.");
                return Redirect("/Home");
            }
            var isSpecialist = this.specialists.IsSpecialist(userId);
            if (isSpecialist)
            {
                TempData.Add("Error", "Specialists cannot subscribe.");
                return Redirect("/Home");
            }
            //Archive appointments
            this.appointments.ArchiveAppointments(userId);
            var activeAppointments = this.appointments.GetUserAppointmetsCount(userId);
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
                model.Subscriptions = this.subscriptions.GetSubscriptions();
                return View(model);
            }
            var isValidSubcription = this.subscriptions.IsValidSubcription(model.SubscriptionId);
            if (!isValidSubcription)
            {
                ModelState.AddModelError(nameof(model.SubscriptionId), "Select a valid subscription plan.");
                model.Subscriptions = this.subscriptions.GetSubscriptions();
                return View(model);
            }
            var userId = this.User.Id();
            await this.subscriptions.SubscribeUser(model.SubscriptionId, userId);
            TempData.Add("Success", "Successful subscription.");

            return Redirect("/Appointments/Make");
        }
    }
}

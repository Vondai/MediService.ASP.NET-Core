using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Models.Appointments;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly MediServiceDbContext data;

        public AppointmentsController(MediServiceDbContext data)
        {
            this.data = data;
        }

        public IActionResult Make()
        {
            var userId = this.User.Id();
            var isSubscriber = this.data
                .Users
                .Any(u => u.Id == userId && u.SubscriptionId.HasValue);
            if (!isSubscriber)
            {
                return Redirect("/Subscriptions/All");
            }

            return View(new AppointmentFormModel() { Services = GetMediServices() });
        }

        private IEnumerable<ServiceViewFormModel> GetMediServices()
        => this.data.Services
            .Select(x => new ServiceViewFormModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();
    }
}

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Infrastructure;

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

            return View();
        }
    }
}

using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Models.Subscriptions;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
    }
}

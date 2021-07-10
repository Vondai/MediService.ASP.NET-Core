using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Subscriptions;

namespace MediService.ASP.NET_Core.Controllers
{
    public class AdminController : Controller
    {
        private readonly MediServiceDbContext data;

        public AdminController(MediServiceDbContext data)
        {
            this.data = data;
        }

        public IActionResult AddSubscription() => View();

        [HttpPost]
        public async Task<IActionResult> AddSubscription(SubscriptionFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var subscription = new Subscription()
            {
                Name = model.Name,
                Price = decimal.Parse(model.Price),
                CountService = model.CountServices,
            };

            this.data.Subscriptions.Add(subscription);
            await this.data.SaveChangesAsync();

            return Redirect("/Subscriptions/All");
        }
    }
}

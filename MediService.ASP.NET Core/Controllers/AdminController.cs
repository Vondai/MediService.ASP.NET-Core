using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Subscriptions;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Controllers
{
    public class AdminController : Controller
    {
        private readonly MediServiceDbContext data;

        public AdminController(MediServiceDbContext data)
        {
            this.data = data;
        }

        [HttpGet]
        public IActionResult AddSubscription() => View();

        [HttpPost]
        public async Task<IActionResult> AddSubscription(SubscriptionAddFormModel model)
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

            var subscription = new Subscription()
            {
                Name = model.Name,
                Price = price,
                CountService = model.CountServices,
            };
            this.data.Subscriptions.Add(subscription);
            await this.data.SaveChangesAsync();

            return Redirect("/Subscriptions/All");
        }

        [HttpGet]
        public IActionResult AddService() => View();

        public async Task<IActionResult> AddService(ServiceAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var service = new Service()
            {
                Name = model.Name,
                Description = model.Description
            };
            await data.Services.AddAsync(service);
            await data.SaveChangesAsync();

            return Redirect("/Services/All");
        }
    }
}

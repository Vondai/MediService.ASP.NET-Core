using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Services;
using MediService.ASP.NET_Core.Models.Specialists;
using MediService.ASP.NET_Core.Models.Subscriptions;

using static MediService.ASP.NET_Core.Areas.Admin.AdminConstants;

namespace MediService.ASP.NET_Core.Controllers
{
    [Authorize(Roles = AdminRoleName)]
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

        public IActionResult AddSpecialist() => View(new SpecialistAddFormModel()
        {
            Services = GetMediServices()
        });

        [HttpPost]
        public async Task<IActionResult> AddSpecialist(SpecialistAddFormModel model, [FromForm] IFormFile specImage)
        {
            if (!ModelState.IsValid)
            {
                model.Services = GetMediServices();
                return View(model);
            }
            var userId = this.data.Users
                .Where(u => u.UserName == model.Username)
                .Select(x => x.Id)
                .FirstOrDefault();
            if (userId == null)
            {
                ModelState.AddModelError(nameof(model.Username), "Invalid username.");
                model.Services = GetMediServices();
                return View(model);
            }
            var mediService = this.data.Services
                .Where(s => s.Id == model.ServiceId)
                .FirstOrDefault();
            if (mediService == null)
            {
                ModelState.AddModelError(nameof(model.ServiceId), "Invalid medical service.");
                model.Services = GetMediServices();
                return View(model);
            }
            string imageUrl = null;
            if (specImage == null || specImage.Length == 0)
            {
                var defualtImg = "default";
                imageUrl = $"/img/{defualtImg}.jpg";
            }
            else
            {
                var fileName = model.Username + "_img.jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img", fileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await specImage.CopyToAsync(fileStream);
                imageUrl = $"/img/{fileName}";
            }
            var specialist = new Specialist()
            {
                UserId = userId,
                Description = model.Description,
                ImageUrl = imageUrl,
            };
            specialist.Services.Add(mediService);
            this.data.Specialists.Add(specialist);

            await this.data.SaveChangesAsync();

            return this.Redirect("/Specialists/All");
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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MediService.ASP.NET_Core.Models.Services;
using MediService.ASP.NET_Core.Services.MedicalServices;

using static MediService.ASP.NET_Core.WebConstants.Cache;
using static MediService.ASP.NET_Core.WebConstants.GlobalMessage;

namespace MediService.ASP.NET_Core.Areas.Admin.Controllers
{
    public class ServicesController : AdminController
    {
        private readonly IMedicalService medicalServices;
        private readonly IMemoryCache cache;

        public ServicesController(IMedicalService medicalServices, IMemoryCache cache)
        {
            this.medicalServices = medicalServices;
            this.cache = cache;
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(ServiceFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await this.medicalServices.CreateService(model.Name, model.Description);

            TempData.Add(SuccessKey, "Successfuly added medical service.");
            return Redirect("/Services/All");
        }

        public IActionResult Edit(int id)
        {
            var service = this.medicalServices.GetById(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult Edit(int id, ServiceFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var edited = this.medicalServices.Edit(id, model.Name, model.Description);

            if (!edited)
            {
                return BadRequest();
            }
            this.cache.Remove(AllServicesKey);

            TempData.Add(SuccessKey, "Medical service successfuly edited.");
            return Redirect("/Services/All");
        }
    }
}

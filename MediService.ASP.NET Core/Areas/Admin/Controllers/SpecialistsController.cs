using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Models.Specialists;
using MediService.ASP.NET_Core.Services.Accounts;
using MediService.ASP.NET_Core.Services.MedicalServices;
using MediService.ASP.NET_Core.Services.Specialists;

namespace MediService.ASP.NET_Core.Areas.Admin.Controllers
{
    public class SpecialistsController : AdminController
    {
        private readonly IMedicalService medicalServices;
        private readonly IAccountService accounts;
        private readonly ISpecialistService specialists;

        public SpecialistsController(
            IMedicalService medicalServices, 
            IAccountService accounts, 
            ISpecialistService specialists)
        {
            this.medicalServices = medicalServices;
            this.accounts = accounts;
            this.specialists = specialists;
        }

        public IActionResult Add() => View(new SpecialistAddFormModel()
        {
            Services = this.medicalServices.GetServices()
        });

        [HttpPost]
        public async Task<IActionResult> Add(SpecialistAddFormModel model, [FromForm] IFormFile specImage)
        {
            if (!ModelState.IsValid)
            {
                model.Services = this.medicalServices.GetServices();
                return View(model);
            }
            var userId = this.accounts.GetIdByUsername(model.Username);
            if (userId == null)
            {
                ModelState.AddModelError(nameof(model.Username), "Invalid username.");
                model.Services = this.medicalServices.GetServices();
                return View(model);
            }
            var mediService = this.medicalServices.GetServiceById(model.ServiceId);
            if (mediService == null)
            {
                ModelState.AddModelError(nameof(model.ServiceId), "Invalid medical service.");
                model.Services = this.medicalServices.GetServices();
                return View(model);
            }
            await this.specialists.CreateSpecialist(userId, model.Username, model.Description, specImage, mediService);

            TempData.Add("Success", "Successfuly added specialist.");
            return this.Redirect("/Specialists/All");
        }
    }
}

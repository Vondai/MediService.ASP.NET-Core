using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Models.Services;
using MediService.ASP.NET_Core.Services.MedicalServices;

namespace MediService.ASP.NET_Core.Areas.Admin.Controllers
{
    public class ServicesController : AdminController
    {
        private readonly IMedicalService medicalServices;

        public ServicesController(IMedicalService medicalServices)
        {
            this.medicalServices = medicalServices;
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(ServiceAddFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await this.medicalServices.CreateService(model.Name, model.Description);

            TempData.Add("Success", "Successfuly added medical service.");
            return Redirect("/Services/All");
        }
    }
}

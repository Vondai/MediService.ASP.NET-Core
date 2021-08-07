using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Models.Specialists;

namespace MediService.ASP.NET_Core.Controllers
{
    public class SpecialistsController : Controller
    {
        private readonly MediServiceDbContext data;

        public SpecialistsController(MediServiceDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            var specialists = this.data
                .Specialists
                .OrderBy(s => s.User.FullName)
                .Select(s => new SpecialistViewModel
                {
                    FullName = s.User.FullName,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl,
                    Services = s.Services.Select(x => x.Name)
                     .ToArray()
                })
                .ToList();

            return View(specialists);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using System.Linq;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Controllers
{
    public class ServicesController : Controller
    {
        private readonly MediServiceDbContext data;

        public ServicesController(MediServiceDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            var services = data.Services
                .Select(x => new ServiceViewModel()
                {
                    Name = x.Name,
                    Description = x.Description,
                })
                .ToList();

            return View(services);
        }
    }
}

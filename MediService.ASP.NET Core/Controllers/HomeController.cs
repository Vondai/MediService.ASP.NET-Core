using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MediService.ASP.NET_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MediServiceDbContext data;

        public HomeController(ILogger<HomeController> logger, MediServiceDbContext data)
        {
            _logger = logger;
            this.data = data;
        }

        public IActionResult Index() => View();

        public IActionResult Faq() => View();

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

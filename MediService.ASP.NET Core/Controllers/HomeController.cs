using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Enums;
using MediService.ASP.NET_Core.Models;
using MediService.ASP.NET_Core.Models.Reviews;

namespace MediService.ASP.NET_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly MediServiceDbContext data;

        public HomeController(MediServiceDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var reviews = this.data.Reviews
                .Where(r => (Rating)r.Rating >= Rating.Excellent)
                .Select(x => new ReviewViewModel()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Rating = ((Rating)x.Rating).ToString(),
                    Username = this.data.Users
                    .Where(u => u.Id == x.UserId)
                    .Select(u => u.UserName)
                    .FirstOrDefault(),
                })
                .Take(6)
                .ToList();

            return View(reviews);
        }

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

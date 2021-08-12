using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Areas.Admin.Services;

namespace MediService.ASP.NET_Core.Areas.Admin.Controllers
{
    public class StatisticsController : AdminController
    {
        private readonly IStatisticsService statistics;

        public StatisticsController(IStatisticsService statistics)
            => this.statistics = statistics;

        public IActionResult GetStatistics()
        {
            var stats = this.statistics.GetStatistics();

            return View(stats);
        }
    }
}

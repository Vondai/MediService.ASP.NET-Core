using MediService.ASP.NET_Core.Areas.Admin.Models.Statistics;

namespace MediService.ASP.NET_Core.Areas.Admin.Services
{
    public interface IStatisticsService
    {
        StatisticsViewModel GetStatistics();
    }
}

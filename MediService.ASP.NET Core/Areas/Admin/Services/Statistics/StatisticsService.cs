using System.Linq;
using MediService.ASP.NET_Core.Areas.Admin.Models.Statistics;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Enums;

namespace MediService.ASP.NET_Core.Areas.Admin.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly MediServiceDbContext data;

        public StatisticsService(MediServiceDbContext data)
            => this.data = data;

        public StatisticsViewModel GetStatistics()
        {
            var stats = new StatisticsViewModel()
            {
                TotalAppointments = this.GetTotalAppointments(),
                CanceledAppointments = this.GetCanceledAppointments(),
                FinishedAppointments = this.GetFinishedAppointments(),
                OverallRating = this.GetOverallRating()
            };

            return stats;
        }
        private int GetTotalAppointments()
            => this.data
            .Appointments
            .Count();

        private int GetCanceledAppointments()
            => this.data
            .Appointments
            .Where(a => a.IsCanceled == true)
            .Count();

        private int GetFinishedAppointments()
            => this.data
            .Appointments
            .Where(a => a.IsDone == true)
            .Count();

        private string GetOverallRating()
        {
            var avgRating = (int)this.data
            .Reviews
            .Select(x => x.Rating)
            .DefaultIfEmpty()
            .Average();
            var rating = string.Empty;
            if (avgRating == 0)
            {
                rating = "Not Rated";
            }
            else
            {
                var enumRating = (Rating)avgRating;
                rating = enumRating.ToString();
            }
            return rating;
        }
    }
}

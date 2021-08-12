namespace MediService.ASP.NET_Core.Areas.Admin.Models.Statistics
{
    public class StatisticsViewModel
    {
        public int TotalAppointments { get; init; }

        public int FinishedAppointments { get; init; }

        public int CanceledAppointments { get; init; }

        public string OverallRating { get; init; }
    }
}

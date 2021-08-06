namespace MediService.ASP.NET_Core.Models.Appointments
{
    public class AppointmentViewModel
    {
        public string Id { get; init; }

        public string Time { get; init; }

        public string IsDone { get; init; }

        public string IsCanceled { get; init; }

        public string ServiceName { get; init; }

        public string Name { get; init; }
    }
}

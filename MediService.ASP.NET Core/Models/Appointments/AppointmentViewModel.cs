namespace MediService.ASP.NET_Core.Models.Appointments
{
    public class AppointmentViewModel
    {
        public string Id { get; init; }

        public string Date { get; init; }

        public string Time { get; init; }

        public string ServiceName { get; init; }

        public bool CanCancel { get; init; }

        public string Name { get; init; }
    }
}

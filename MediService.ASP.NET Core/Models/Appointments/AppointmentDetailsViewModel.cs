namespace MediService.ASP.NET_Core.Models.Appointments
{
    public class AppointmentDetailsViewModel
    {
        public string Id { get; init; }
        public string PatientName { get; init; }

        public string Address { get; init; }

        public string City { get; init; }

        public string Date { get; init; }

        public string AdditionalInfo { get; init; }

        public string Service { get; init; }

        public string PhoneNumber { get; init; }

        public string Email { get; init; }
    }
}

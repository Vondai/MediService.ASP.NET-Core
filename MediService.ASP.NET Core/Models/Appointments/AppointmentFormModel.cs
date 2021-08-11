using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediService.ASP.NET_Core.Models.Services;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Models.Appointments
{
    public class AppointmentFormModel
    {
        public string Address { get; set; }

        [Display(Name = "Additional information")]
        [MaxLength(AppointmentInfoMaxLength)]
        public string AdditionalInfo { get; set; }

        [Required(ErrorMessage = "Please choose a date and time for the appointment")]
        public string Date { get; set; }

        [Display(Name = "Medical service")]
        [Required(ErrorMessage = "Please choose a medical service")]
        public int ServiceId { get; init; }

        public int AppointmentsLeft { get; set; }

        public IEnumerable<ServiceViewFormModel> Services { get; set; }
    }
}

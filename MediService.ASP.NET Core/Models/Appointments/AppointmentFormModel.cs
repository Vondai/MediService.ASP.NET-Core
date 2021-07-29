using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Models.Appointments
{
    public class AppointmentFormModel
    {
        public string Address { get; set; }

        [Display(Name = "Additional nformation")]
        public string AdditionalInfo { get; set; }

        [Required(ErrorMessage = "Please choo a time for the appointment")]
        public string Time { get; set; }

        [Display(Name = "Medical service")]
        public int ServiceId { get; init; }

        public IEnumerable<ServiceViewFormModel> Services { get; set; }
    }
}

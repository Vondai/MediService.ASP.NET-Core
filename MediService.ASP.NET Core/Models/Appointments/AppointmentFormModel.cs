using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Models.Appointments
{
    public class AppointmentFormModel
    {
        public string Time { get; set; }

        [Display(Name = "Medical service")]
        public int ServiceId { get; init; }

        public IEnumerable<ServiceViewFormModel> Services { get; set; }
    }
}

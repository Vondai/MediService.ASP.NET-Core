using System;
using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Appointment
    {
        public Appointment()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsDone { get; set; }

        public bool IsCanceled { get; set; }

        [MaxLength(AppointmentInfoMaxLength)]
        public string AdditionalInfo { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public Service Service { get; init; }

        [Required]
        public string UserId { get; set; }

        public User User { get; init; }

        [Required]
        public string SpecialistId { get; set; }

        public Specialist Specialist { get; set; }

    }
}

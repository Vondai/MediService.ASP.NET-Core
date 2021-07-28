using System;
using System.ComponentModel.DataAnnotations;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Appointment
    {
        public int Id { get; init; }

        public DateTime Time { get; set; }

        public bool IsDone { get; set; }

        [Required]
        public int ServiceId { get; set; }

        public Service Service { get; init; }

        [Required]
        public string UserId { get; set; }

        public User User { get; init; }
    }
}

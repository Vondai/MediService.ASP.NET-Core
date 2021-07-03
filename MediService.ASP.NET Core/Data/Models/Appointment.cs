using System;
using System.ComponentModel.DataAnnotations;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Appointment
    {
        public int Id { get; init; }

        public DateTime Time { get; init; }

        public bool IsDone { get; init; }

        [Required]
        public int ServiceId { get; init; }

        public Service Service { get; init; }

        [Required]
        public string UserId { get; init; }

        public User User { get; init; }
    }
}

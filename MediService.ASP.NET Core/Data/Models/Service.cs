using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Service
    {
        public Service()
        {
            this.Specialists = new HashSet<Specialist>();
            this.Appointments = new HashSet<Appointment>();

        }
        public int Id { get; init; }

        [Required]
        public string Name { get; init; }

        public string Description { get; init; }

        public ICollection<Specialist> Specialists { get; init; }

        public ICollection<Appointment> Appointments { get; init; }
    }
}

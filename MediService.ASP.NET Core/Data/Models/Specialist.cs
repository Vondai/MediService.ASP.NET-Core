using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Specialist
    {
        public Specialist()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Services = new HashSet<Service>();
        }
        public string Id { get; init; }

        [Required]
        [MaxLength(SpecialistDescriptionMaxLength)]
        public string Description { get; init; }

        [Required]
        public string ImageUrl { get; init; }

        [Required]
        public string UserId { get; init; }

        public User User { get; init; }

        public ICollection<Service> Services { get; init; }
    }
}

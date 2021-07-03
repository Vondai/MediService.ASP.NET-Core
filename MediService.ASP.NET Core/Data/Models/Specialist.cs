using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string Description { get; init; }

        public string ImageUrl { get; init; }

        [Required]
        public string UserId { get; init; }

        public User User { get; init; }

        public ICollection<Service> Services { get; init; }
    }
}

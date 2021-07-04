using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Messages = new HashSet<Message>();
            this.Appointments = new HashSet<Appointment>();
            this.Addresses = new HashSet<Address>();
        }

        public string Id { get; init; }

        [Required]
        public string Username { get; init; }

        [Required]
        public string FullName { get; init; }

        [Required]
        public string Password { get; init; }

        [Required]
        public string PhoneNumber { get; init; }

        [Required]
        public string Email { get; init; }

        public bool IsSpecialist { get; init; }

        public string SpecialistId { get; init; }

        public Specialist Specialist { get; init; }

        public int? SubscriptionId { get; init; }

        public Subscription Subscription { get; init; }

        public ICollection<Message> Messages { get; init; }

        public ICollection<Appointment> Appointments { get; init; }

        public ICollection<Address> Addresses { get; init; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MediService.ASP.NET_Core.Data.Models
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        public User()
        {
            this.Messages = new HashSet<Message>();
            this.Appointments = new HashSet<Appointment>();
            this.Addresses = new HashSet<Address>();
            this.Reviews = new HashSet<Review>();
        }

        [Required]
        public string FullName { get; init; }

        public bool IsSpecialist { get; init; }

        public string SpecialistId { get; init; }

        public Specialist Specialist { get; init; }

        public int? SubscriptionId { get; init; }

        public Subscription Subscription { get; init; }

        public ICollection<Message> Messages { get; init; }

        public ICollection<Appointment> Appointments { get; init; }

        public ICollection<Address> Addresses { get; init; }

        public ICollection<Review> Reviews { get; init; }
    }
}

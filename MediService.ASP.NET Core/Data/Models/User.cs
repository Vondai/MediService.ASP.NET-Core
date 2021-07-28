﻿using System.Collections.Generic;
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
        public string FullName { get; set; }

        public bool IsSpecialist { get; set; }

        public string SpecialistId { get; set; }

        public Specialist Specialist { get; set; }

        public int? SubscriptionId { get; set; }

        public Subscription Subscription { get; set; }

        public ICollection<Message> Messages { get; init; }

        public ICollection<Appointment> Appointments { get; init; }

        public ICollection<Address> Addresses { get; init; }

        public ICollection<Review> Reviews { get; init; }
    }
}

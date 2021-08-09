using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Subscription
    {
        public Subscription()
        {
            this.Users = new HashSet<User>();
        }
        public int Id { get; init; }

        [Required]
        [MaxLength(SubscriptionNameMaxLength)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public int AppointmentCount { get; set; }

        public ICollection<User> Users { get; init; }
    }
}

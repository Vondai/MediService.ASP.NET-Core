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

        public string Name { get; init; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; init; }

        public int CountService { get; init; }

        public ICollection<User> Users { get; init; }
    }
}

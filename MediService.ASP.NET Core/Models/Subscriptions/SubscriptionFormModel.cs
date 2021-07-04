using System.ComponentModel.DataAnnotations;

namespace MediService.ASP.NET_Core.Models.Subscriptions
{
    public class SubscriptionFormModel
    {
        [Required]

        public string Name { get; init; }

        public string Price { get; init; }

        public int CountServices { get; init; }
    }
}

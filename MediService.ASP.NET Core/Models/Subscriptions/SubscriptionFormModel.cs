using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;
namespace MediService.ASP.NET_Core.Models.Subscriptions
{
    public class SubscriptionFormModel
    {
        [Required]
        public string Name { get; init; }

        [Required]
        public string Price { get; init; }

        [Required(ErrorMessage = SubscriptionCountServicesNullErrorMessage)]
        [Display(Name = "Count or survices per user")]
        public int CountServices { get; init; }
    }
}

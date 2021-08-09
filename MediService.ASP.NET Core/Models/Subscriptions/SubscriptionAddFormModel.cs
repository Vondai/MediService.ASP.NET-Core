using System.ComponentModel.DataAnnotations;
using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Models.Subscriptions
{
    public class SubscriptionAddFormModel
    {
        [Required]
        [StringLength(
            SubscriptionNameMaxLength,
            MinimumLength = SubsciptionNameMinLength,
            ErrorMessage = "Name must be atleast {2} characters long.")]
        public string Name { get; init; }

        [Required]
        public string Price { get; init; }

        [Required(ErrorMessage = SubscriptionCountAppointmentNullErrorMessage)]
        [Display(Name = "Count of appointments per user")]
        public int AppointmentCount { get; init; }
    }
}

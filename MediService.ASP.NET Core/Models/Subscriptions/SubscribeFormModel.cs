using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediService.ASP.NET_Core.Models.Subscriptions
{
    public class SubscribeFormModel
    {
        [Required]
        [CreditCard(ErrorMessage = "Invalid card number.")]
        [Display(Name = "Credit card")]
        public string CreditCard { get; init; }

        [Required]
        [RegularExpression("[0-9]{3}", ErrorMessage = "Invalid Cvc.")]
        public string Cvc { get; init; }

        [Display(Name = "Subscription Plan")]
        public int SubscriptionId { get; init; }

        public Dictionary<int, SubscriptionFormModel> Subscriptions { get; set; }
    }
}

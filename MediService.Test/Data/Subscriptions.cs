using System.Collections.Generic;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Subscriptions;

namespace MediService.Test.Data
{
    public class Subscriptions
    {
        public static ICollection<Subscription> ThreeSubscriptions()
            => new List<Subscription>
                {
                    new Subscription() {Id = 1},
                    new Subscription() {Id = 2},
                    new Subscription() {Id = 3},
                };

        public static SubscribeFormModel SubscriptionFormModelTest()
        {
            var formModel = new SubscribeFormModel()
            {
                Subscriptions = new Dictionary<int, SubscriptionFormModel>()
            };

            return formModel;
        }

        public static SubscribeFormModel ValidSubscribeFormModel()
        {
            var formModel = new SubscribeFormModel()
            {
                CreditCard = "1111222233334444",
                Cvc = "111",
                SubscriptionId = 1
            };

            return formModel;
        }

        public static SubscribeFormModel InvalidCardAndCvc()
        {
            var formModel = new SubscribeFormModel()
            {
                CreditCard = "",
                Cvc = "",
                SubscriptionId = 1
            };

            return formModel;
        }

        public static SubscribeFormModel InvalidSubscription()
        {
            var formModel = new SubscribeFormModel()
            {
                CreditCard = "1111222233334444",
                Cvc = "111",
                SubscriptionId = 10
            };

            return formModel;
        }
    }
}

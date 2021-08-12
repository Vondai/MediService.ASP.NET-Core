using MediService.ASP.NET_Core.Models.Subscriptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediService.ASP.NET_Core.Services.Subscriptions
{
    public interface ISubscriptionService
    {
        Task<int> CreateSubscription(
            string name,
            decimal price,
            int appointmentCount);

        ICollection<SubscriptionViewModel> GetAll();

        bool IsValidSubcription(int subscriptionId);

        bool IsSubscriber(string userId);

        int GetSubscriptionAppointmentCount(string userId);


        Task SubscribeUser(int subscriptionId, string userId);

        Dictionary<int, SubscriptionFormModel> GetSubscriptions();
    }
}

using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Subscriptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediService.ASP.NET_Core.Services.Subscriptions
{
    public interface ISubscriptionService
    {
        int ActiveAppointments(string userId);

        ICollection<SubscriptionViewModel> GetAll();

        Subscription GetSubscription(int subscriptionId);


        Task SubscribeUser(Subscription newSubscription, User user);

        Dictionary<int, SubscriptionFormModel> GetSubscriptions();
    }
}

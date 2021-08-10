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

        bool IsValidSubcription(int subscriptionId);


        Task SubscribeUser(int subscriptionId, string userId);

        Dictionary<int, SubscriptionFormModel> GetSubscriptions();
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Subscriptions;

namespace MediService.ASP.NET_Core.Services.Subscriptions
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly MediServiceDbContext data;

        public SubscriptionService(MediServiceDbContext data)
        {
            this.data = data;
        }

        public int ActiveAppointments(string userId)
            => this.data
                .Appointments
                .Where(a => a.User.Id == userId
                    && a.IsCanceled == false
                    && a.IsDone == false)
                .Count();

        public ICollection<SubscriptionViewModel> GetAll()
            => this.data.Subscriptions
                .OrderBy(x => x.Price)
                .Select(s => new SubscriptionViewModel()
                {
                    Name = s.Name,
                    Price = s.Price.ToString(),
                    AppointmentCount = s.AppointmentCount.ToString(),
                })
                .ToList();

        public Subscription GetSubscription(int subscriptionId)
            => this.data
                .Subscriptions
                .FirstOrDefault(x => x.Id == subscriptionId);

        public Dictionary<int, SubscriptionFormModel> GetSubscriptions()
            => this.data
                .Subscriptions
                .OrderBy(s => s.Price)
                .Select(x => new SubscriptionFormModel()
                {
                    Id = x.Id,
                    AppointmentCount = x.AppointmentCount,
                    Name = x.Name,
                    Price = x.Price.ToString(),
                })
                .ToDictionary(x => x.Id);

        public async Task SubscribeUser(Subscription newSubscription, User user)
        {
            var currentSubscription = this.GetUserSubscription(user);
            if (currentSubscription != null)
            {
                currentSubscription.Users.Remove(user);
            }
            newSubscription.Users.Add(user);
            await this.data.SaveChangesAsync();
        }

        private Subscription GetUserSubscription(User user)
            => this.data
                .Subscriptions
                .Where(x => x.Users.Contains(user))
                .FirstOrDefault();
    }
}

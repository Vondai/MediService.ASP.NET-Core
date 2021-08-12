﻿using System.Collections.Generic;
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

        public async Task<int> CreateSubscription(string name, decimal price, int appointmentCount)
        {
            var subscription = new Subscription()
            {
                Name = name,
                Price = price,
                AppointmentCount = appointmentCount,
            };
            this.data.Subscriptions.Add(subscription);
            await this.data.SaveChangesAsync();

            return subscription.Id;
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

        public bool IsValidSubcription(int subscriptionId)
            => this.data
                .Subscriptions
                .Any(x => x.Id == subscriptionId);

        public bool IsSubscriber(string userId)
            => this.data
                .Users
                .Any(u => u.Id == userId && u.SubscriptionId.HasValue);

        public int GetSubscriptionAppointmentCount(string userId)
            => this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(x => x.Subscription.AppointmentCount)
                .FirstOrDefault();

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

        public async Task SubscribeUser(int subscriptionId, string userId)
        {
            var user = this.data
                .Users
                .FirstOrDefault(u => u.Id == userId);
            user.SubscriptionId = subscriptionId;
            await this.data.SaveChangesAsync();
        }
    }
}

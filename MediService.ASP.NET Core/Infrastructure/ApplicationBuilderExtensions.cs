using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using System;

namespace MediService.ASP.NET_Core.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<MediServiceDbContext>();

            data.Database.Migrate();

            SeedMediServices(data);
            SeedSubscriptions(data);

            return app;
        }

        private static void SeedSubscriptions(MediServiceDbContext data)
        {
            if (data.Subscriptions.Any())
            {
                return;
            }
            data.Subscriptions
                .AddRange(new[]
                {
                    new Subscription {Name = "Basic", Price = 39.99M, CountService = 2},
                    new Subscription {Name = "Standard", Price = 49.99M, CountService = 3},
                    new Subscription {Name = "Premium", Price = 59.99M, CountService = 5},
                });
            data.SaveChanges();
        }

        private static void SeedMediServices(MediServiceDbContext data)
        {
            if (data.Services.Any())
            {
                return;
            }

            data.Services.AddRange(new[]
            {
                new Service {Name = "Doctor care", Description = "A doctor may visit you at home to diagnose and treat the illness(es). He or she may also periodically review the home health care needs."},
                new Service {Name = "Nurse Practitioner", Description = "Delivery of comprehensive health care in the comfort of your own home."},
                new Service {Name = "Around The Clock Care", Description = "All the care and support you need, in the home you love."},
                new Service {Name = "Care after Surgery", Description = "By your side, providing peace of mind and care."},
                new Service {Name = "Therapy & Rehab Services", Description = "Therapy and Rehab Services at home."},
                new Service {Name = "Nutritional support", Description = "Dietitians can come to a patient's home to provide dietary assessments and guidance to support the treatment plan."},
            });

            data.SaveChanges();
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;

using static MediService.ASP.NET_Core.Areas.Admin.AdminConstants;

namespace MediService.ASP.NET_Core.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var services = scopedServices.ServiceProvider;

            MigrateDatabase(services);

            SeedMediServices(services);
            SeedSubscriptions(services);
            SeedUsers(services);
            SeedSpecialists(services);
            SeedAdmin(services);

            return app;
        }

        private static void SeedAdmin(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdminRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@test.co";
                    const string adminPassword = "123456";
                    const string adminUsername = "admin";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminUsername,
                        FullName = "Admin",
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedSpecialists(IServiceProvider services)
        {
            var data = services.GetRequiredService<MediServiceDbContext>();
            if (data.Specialists.Any())
            {
                return;
            }
            var servicesCount = data.Services.Count();
            var usernames = new string[] { "JohnMD", "SaraNurse", "OliviaCare", "MattSurgery", "OliverRehab", "PhilNutri" };
            var descriptions = new string[]
            {
                "John Smith has worked with patients for more than 30 years.",
                "Sara is one the best nurses in the world.",
                "When you need everyday care in your home Olivia Brown is here for you.",
                "Matt River is one of the best care takers after a surgery.",
                "Be safe with Oliver Bake.",
                "Phil Williams will make you a personal diet plan that you can follow with ease."
            };
            var servicesNames = new string[]
            {
                "Doctor care",
                "Nurse Practitioner",
                "Around The Clock Care",
                "Care after Surgery",
                "Therapy & Rehab Services",
                "Nutritional support"
            };
            for (int i = 0; i < servicesCount; i++)
            {
                var specialist = new Specialist
                {
                    Description = descriptions[i],
                    UserId = data.Users.Where(u => u.UserName == usernames[i]).Select(x => x.Id).First(),
                    ImageUrl = $"/img/{usernames[i]}_img.jpg",
                };
                specialist.Services.Add(data.Services.Where(s => s.Name == servicesNames[i]).FirstOrDefault());

                data.Specialists
                    .Add(specialist);
                data.SaveChanges();
            }
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<MediServiceDbContext>();
            data.Database.Migrate();
        }

        private static void SeedUsers(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var data = services.GetRequiredService<MediServiceDbContext>();
            if (data.Users.Any())
            {
                return;
            }
            var servicesCount = data.Services.Count();
            var usernames = new string[] { "JohnMD", "SaraNurse", "OliviaCare", "MattSurgery", "OliverRehab", "PhilNutri" };
            var fullNames = new string[] { "John Smith", "Sara", "Olivia Brown", "Matt River", "Oliver Bake", "Phil Williams" };
            var password = "123456";
            var city = "Sofia";
            Task
                .Run(async () =>
                {
                    for (int i = 0; i < servicesCount; i++)
                    {
                        var address = new Address() { City = city, FullAddress = $"1{i} some str." };
                        var user = new User
                        {
                            Email = $"spec{i}@test.co",
                            FullName = fullNames[i],
                            UserName = usernames[i],
                        };
                        user.Addresses.Add(address);
                        await userManager.CreateAsync(user, password);
                    }
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedSubscriptions(IServiceProvider services)
        {
            var data = services.GetRequiredService<MediServiceDbContext>();

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

        private static void SeedMediServices(IServiceProvider services)
        {
            var data = services.GetRequiredService<MediServiceDbContext>();
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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Services.MedicalServices;
using MediService.ASP.NET_Core.Services.Reviews;
using MediService.ASP.NET_Core.Services.Specialists;
using MediService.ASP.NET_Core.Services.Subscriptions;
using MediService.ASP.NET_Core.Services.Appointments;
using MediService.ASP.NET_Core.Services.Accounts;
using MediService.ASP.NET_Core.Areas.Admin.Services;

namespace MediService.ASP.NET_Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //Register dbContext
            services.AddDbContext<MediServiceDbContext>(o =>
                o.UseSqlServer(Configuration.GetConnectionString("Default")));

            //Register Identity Service
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<MediServiceDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.Lockout.AllowedForNewUsers = false;
            });
            services.AddMemoryCache();
            services.AddTransient<ISpecialistService, SpecialistService>();
            services.AddTransient<IReviewService, ReviewService>();
            services.AddTransient<IMedicalService, MedicalService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IStatisticsService, StatisticsService>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //ApplyMigration
            app.PrepareDatabase();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Enable users
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultAreaRoute();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

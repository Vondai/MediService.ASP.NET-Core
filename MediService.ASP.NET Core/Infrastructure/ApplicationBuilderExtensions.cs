using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MediService.ASP.NET_Core.Data;
using Microsoft.EntityFrameworkCore;

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

            return app;
        }
    }
}

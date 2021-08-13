using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MediService.ASP.NET_Core.Services.Appointments;

namespace MediService.ASP.NET_Core.Services.Background
{
    public class Worker : IWorker
    {
        private readonly IServiceProvider provider;

        public Worker(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public async Task DoWork(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = this.provider.CreateScope();
                var appointments = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                await appointments.ArchiveAppointments();
                await Task.Delay(TimeSpan.FromMinutes(10), cancellationToken);
            }
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MediService.ASP.NET_Core.Services.Background
{
    public class ArchiveService : BackgroundService
    {
        private readonly IWorker worker;

        public ArchiveService(IWorker worker)
            => this.worker = worker;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.worker.DoWork(stoppingToken);
        }
    }
}

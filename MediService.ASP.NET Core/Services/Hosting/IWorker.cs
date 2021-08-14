using System.Threading;
using System.Threading.Tasks;

namespace MediService.ASP.NET_Core.Services.Hosting
{
    public interface IWorker
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}

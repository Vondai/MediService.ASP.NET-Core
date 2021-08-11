using System.Linq;
using MediService.ASP.NET_Core.Data;

namespace MediService.ASP.NET_Core.Services.Specialists
{
    public class SpecialistService : ISpecialistService
    {
        private readonly MediServiceDbContext data;

        public SpecialistService(MediServiceDbContext data)
        {
            this.data = data;
        }

        public string GetIdFromService(int serviceId)
        => this.data.Specialists
                .Where(x => x.Services
                .Any(s => s.Id == serviceId))
                .Select(x => x.Id)
                .FirstOrDefault();

        public string IdByUser(string userId)
        => this.data.
            Specialists
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefault();

        public bool IsSpecialist(string userId)
        => this.data
        .Specialists.Any(x => x.UserId == userId);
    }
}

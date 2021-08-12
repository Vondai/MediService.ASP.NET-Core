using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Services.MedicalServices
{
    public class MedicalService : IMedicalService
    {
        private readonly MediServiceDbContext data;

        public MedicalService(MediServiceDbContext data)
        {
            this.data = data;
        }

        public async Task<int> CreateService(string name, string description)
        {
            var service = new Service()
            {
                Name = name,
                Description = description
            };
            await data.Services.AddAsync(service);
            await data.SaveChangesAsync();

            return service.Id;
        }

        public bool IsValidService(int serviceId)
            => this.data.Services
                .Any(x => x.Id == serviceId);

        public Service GetServiceById(int serviceId)
            => this.data.Services
                .Where(s => s.Id == serviceId)
                .FirstOrDefault();

        public ICollection<ServiceViewModel> GetAll()
            => this.data.Services
                    .OrderBy(x => x.Name)
                    .Select(x => new ServiceViewModel()
                    {
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList();

        public ICollection<ServiceViewFormModel> GetServices()
            => this.data.Services
               .Select(x => new ServiceViewFormModel
               {
                   Id = x.Id,
                   Name = x.Name
               })
               .ToList();
    }
}

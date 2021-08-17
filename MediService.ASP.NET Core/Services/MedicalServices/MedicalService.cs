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

        public async Task<int> CreateService(string name, string description, bool isFree)
        {
            var service = new Service()
            {
                Name = name,
                Description = description,
                IsFree = isFree
            };
            await data.Services.AddAsync(service);
            await data.SaveChangesAsync();

            return service.Id;
        }

        public bool Edit(int id, string name, string description, bool isFree)
        {
            var medicalService = this.data.Services
                .Find(id);

            if (medicalService == null)
            {
                return false;
            }

            medicalService.Name = name;
            medicalService.Description = description;
            medicalService.IsFree = isFree;

            this.data.SaveChanges();

            return true;
        }

        public bool IsValidService(int serviceId, bool isFree = false)
            => this.data.Services
                .Where(s => s.IsFree == isFree)
                .Any(x => x.Id == serviceId);

        public ServiceFormModel GetFormModelById(int serviceId)
             => this.data.Services
             .Where(s => s.Id == serviceId)
             .Select(x => new ServiceFormModel()
             {
                 Name = x.Name,
                 Description = x.Description,
                 IsFree = x.IsFree
             })
             .FirstOrDefault();

        public Service GetById(int serviceId)
            => this.data.Services
                .Where(s => s.Id == serviceId)
                .FirstOrDefault();

        public ICollection<ServiceViewModel> GetListing()
            => this.data.Services
                    .OrderBy(x => x.Name)
                    .Select(x => new ServiceViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList();

        public ICollection<ServiceViewFormModel> GetServices(bool isFree = false)
            => this.data.Services
               .Where(s => s.IsFree == isFree)
               .Select(x => new ServiceViewFormModel
               {
                   Id = x.Id,
                   Name = x.Name,
               })
               .ToList();
    }
}

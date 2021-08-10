using System.Collections.Generic;
using System.Linq;
using MediService.ASP.NET_Core.Data;
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

        public ICollection<ServiceViewModel> GetAll()
            => this.data.Services
                    .OrderBy(x => x.Name)
                    .Select(x => new ServiceViewModel()
                    {
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList();
    }
}

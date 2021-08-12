using System.Collections.Generic;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Services.MedicalServices
{
    public interface IMedicalService
    {
        Task<int> CreateService(string name, string description);

        Service GetServiceById(int serviceId);

        bool IsValidService(int serviceId);

        ICollection<ServiceViewModel> GetAll();

        ICollection<ServiceViewFormModel> GetServices();
    }
}

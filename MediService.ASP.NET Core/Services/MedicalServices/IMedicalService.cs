using System.Collections.Generic;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Services.MedicalServices
{
    public interface IMedicalService
    {
        Task<int> CreateService(string name, string description, bool isFree);

        Service GetServiceById(int serviceId);

        ServiceFormModel GetById(int serviceId);

        bool IsValidService(int serviceId);

        bool IsValidFreeService(int serviceId);

        bool Edit(int id, string name, string description, bool isFree);

        ICollection<ServiceViewModel> GetAll();

        ICollection<ServiceViewFormModel> GetServices();

        ICollection<ServiceViewFormModel> GetFreeServices();
    }
}

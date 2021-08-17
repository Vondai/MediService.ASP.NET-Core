using System.Collections.Generic;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.ASP.NET_Core.Services.MedicalServices
{
    public interface IMedicalService
    {
        Task<int> CreateService(string name, string description, bool isFree);

        bool Edit(int id, string name, string description, bool isFree);

        Service GetById(int serviceId);

        ServiceFormModel GetFormModelById(int serviceId);

        bool IsValidService(int serviceId, bool isFree = false);

        ICollection<ServiceViewModel> GetListing();

        ICollection<ServiceViewFormModel> GetServices(bool isFree = false);
    }
}

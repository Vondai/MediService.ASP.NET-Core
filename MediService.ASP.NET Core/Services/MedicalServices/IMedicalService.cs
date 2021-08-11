using MediService.ASP.NET_Core.Models.Services;
using System.Collections.Generic;

namespace MediService.ASP.NET_Core.Services.MedicalServices
{
    public interface IMedicalService
    {
        bool IsValidService(int serviceId);

        ICollection<ServiceViewModel> GetAll();

        ICollection<ServiceViewFormModel> GetServices();
    }
}

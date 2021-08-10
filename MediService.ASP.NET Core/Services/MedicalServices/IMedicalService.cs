using MediService.ASP.NET_Core.Models.Services;
using System.Collections.Generic;

namespace MediService.ASP.NET_Core.Services.MedicalServices
{
    public interface IMedicalService
    {
        ICollection<ServiceViewModel> GetAll();
    }
}

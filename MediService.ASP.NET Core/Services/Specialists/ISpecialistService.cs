using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Specialists;

namespace MediService.ASP.NET_Core.Services.Specialists
{
    public interface ISpecialistService
    {
        ICollection<SpecialistViewModel> GetAll();
        Task<string> CreateSpecialist(
            string userId,
            string username,
            string description,
            IFormFile specImage,
            Service service);
        public bool IsSpecialist(string userId);

        public string IdByUser(string userId);

        public string GetIdFromService(int serviceId);
    }
}

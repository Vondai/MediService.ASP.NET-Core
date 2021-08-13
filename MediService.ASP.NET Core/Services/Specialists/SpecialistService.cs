using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using System.Collections.Generic;
using MediService.ASP.NET_Core.Models.Specialists;

namespace MediService.ASP.NET_Core.Services.Specialists
{
    public class SpecialistService : ISpecialistService
    {
        private readonly MediServiceDbContext data;

        public SpecialistService(MediServiceDbContext data)
        {
            this.data = data;
        }

        public ICollection<SpecialistViewModel> GetAll()
            => this.data
                .Specialists
                .OrderBy(s => s.User.FullName)
                .Select(s => new SpecialistViewModel
                {
                    FullName = s.User.FullName,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl,
                    Services = s.Services.Select(x => x.Name)
                     .ToArray()
                })
                .ToList();

        public async Task<string> CreateSpecialist(
            string userId,
            string username,
            string description,
            IFormFile specImage,
            Service service)
        {
            string imageUrl = null;
            if (specImage == null || specImage.Length == 0)
            {
                var defualtImg = "default";
                imageUrl = $"/img/{defualtImg}.jpg";
            }
            else
            {
                var fileName = username + "_img.jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img", fileName);
                using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
                await specImage.CopyToAsync(fileStream);
                imageUrl = $"/img/{fileName}";
            }
            var specialist = new Specialist()
            {
                UserId = userId,
                Description = description,
                ImageUrl = imageUrl,
            };
            specialist.Services.Add(service);
            this.data.Specialists.Add(specialist);
            await this.data.SaveChangesAsync();

            return specialist.Id;
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

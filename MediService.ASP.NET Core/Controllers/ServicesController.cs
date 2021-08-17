using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MediService.ASP.NET_Core.Models.Services;
using MediService.ASP.NET_Core.Services.MedicalServices;

using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.ASP.NET_Core.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IMedicalService medicalServices;
        private readonly IMemoryCache cache;
        public ServicesController(IMemoryCache cache, IMedicalService medicalServices)
        {
            this.cache = cache;
            this.medicalServices = medicalServices;
        }
        [AllowAnonymous]
        public IActionResult All()
        {
            var services = this.cache.Get<ICollection<ServiceViewModel>>(AllServicesKey);
            if (services == null)
            {
                services = this.medicalServices.GetListing();

                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(1));
                this.cache.Set(AllServicesKey, services, options);
            }

            return View(services);
        }
    }
}

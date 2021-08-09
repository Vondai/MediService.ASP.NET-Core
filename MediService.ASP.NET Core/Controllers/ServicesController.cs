using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Models.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.ASP.NET_Core.Controllers
{
    public class ServicesController : Controller
    {
        private readonly MediServiceDbContext data;
        private readonly IMemoryCache cache;
        public ServicesController(MediServiceDbContext data, IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;
        }
        [AllowAnonymous]
        public IActionResult All()
        {
            var services = this.cache.Get<List<ServiceViewModel>>(AllServicesCacheKey);
            if (services == null)
            {
                services = data.Services
                    .OrderBy(x => x.Name)
                    .Select(x => new ServiceViewModel()
                    {
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList();
                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromDays(1));
                this.cache.Set(AllServicesCacheKey, services, options);
            }

            return View(services);
        }
    }
}

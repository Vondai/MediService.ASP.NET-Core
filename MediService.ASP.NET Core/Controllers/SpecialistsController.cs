using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Models.Specialists;

using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.ASP.NET_Core.Controllers
{
    public class SpecialistsController : Controller
    {
        private readonly MediServiceDbContext data;
        private readonly IMemoryCache cache;

        public SpecialistsController(MediServiceDbContext data, IMemoryCache cache)
        {
            this.data = data;
            this.cache = cache;
        }

        [AllowAnonymous]
        public IActionResult All()
        {
            var specialists = this.cache.Get<List<SpecialistViewModel>>(AllSpecialistsKey);
            if (specialists == null)
            {
                specialists = this.data
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
                var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(1));
                this.cache.Set(AllSpecialistsKey, specialists, options);
            }

            return View(specialists);
        }
    }
}

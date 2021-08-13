using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MediService.ASP.NET_Core.Models.Specialists;

using static MediService.ASP.NET_Core.WebConstants.Cache;
using MediService.ASP.NET_Core.Services.Specialists;

namespace MediService.ASP.NET_Core.Controllers
{
    public class SpecialistsController : Controller
    {
        private readonly IMemoryCache cache;
        private readonly ISpecialistService specialists;

        public SpecialistsController(IMemoryCache cache, ISpecialistService specialists)
        {
            this.cache = cache;
            this.specialists = specialists;
        }

        [AllowAnonymous]
        public IActionResult All()
        {
            var specialists = this.cache.Get<List<SpecialistViewModel>>(AllSpecialistsKey);
            if (specialists == null)
            {
                this.specialists.GetAll();
                var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(1));
                this.cache.Set(AllSpecialistsKey, specialists, options);
            }

            return View(specialists);
        }
    }
}

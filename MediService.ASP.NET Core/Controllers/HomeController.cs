using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MediService.ASP.NET_Core.Models.Reviews;
using MediService.ASP.NET_Core.Services.Reviews;

using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.ASP.NET_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReviewService reviews;
        private readonly IMemoryCache cache;

        public HomeController(IReviewService reviews, IMemoryCache cache)
        {
            this.reviews = reviews;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            var reviews = this.cache.Get<ICollection<ReviewViewModel>>(RecentReviewsCacheKey);
            if (reviews == null)
            {
                reviews = this.reviews.GetRecent();

                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                this.cache.Set(RecentReviewsCacheKey, reviews, options);
            }

            return View(reviews);
        }

        public IActionResult Faq() => View();

        public IActionResult Error()
        {
            return View();
        }
    }
}

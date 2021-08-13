using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Models.Reviews;

using static MediService.Test.Data.Reviews;
using static MediService.ASP.NET_Core.WebConstants.Cache;
using System;

namespace MediService.Test.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnViewWithRecentReviews()
            => MyController<HomeController>
            .Instance(i => i
            .WithData(TenReviews))
            .Calling(c => c.Index())
            .ShouldHave()
            .MemoryCache(cache => cache
            .ContainingEntry(e => e
                .WithKey(RecentReviewsKey)
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(10))
                .WithValueOfType<List<ReviewViewModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<ReviewViewModel>>()
                .Passing(v => v.Should().HaveCount(4)));

        [Fact]
        public void FaqShouldReturnView()
            => MyController<HomeController>
            .Instance()
            .Calling(c => c.Faq())
            .ShouldReturn()
            .View();

    }
}

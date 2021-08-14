using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Models.Specialists;

using static MediService.Test.Data.Specialists;
using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.Test.Controllers
{
    public class SpecialistsControllerTests
    {
        [Fact]
        public void AllShouldReturnViewWithAllSpecialists()
            => MyController<SpecialistsController>
            .Instance()
            .WithData(TenSpecialists())
            .Calling(c => c.All())
            .ShouldHave()
            .MemoryCache(cache => cache
            .ContainingEntry(e => e
                .WithKey(AllSpecialistsKey)
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromDays(1))
                .WithValueOfType<List<SpecialistViewModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<SpecialistViewModel>>());
    }
}

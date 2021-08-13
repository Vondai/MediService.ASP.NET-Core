using System;
using System.Collections.Generic;
using MyTested.AspNetCore.Mvc;
using FluentAssertions;
using Xunit;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Models.Services;

using static MediService.Test.Data.Services;
using static MediService.ASP.NET_Core.WebConstants.Cache;

namespace MediService.Test.Controllers
{
    public class ServicesControllerTests
    {
        [Fact]
        public void AllShouldReturnViewWithAllMedicalServices()
            => MyController<ServicesController>
            .Instance(i => i
            .WithData(TenServices))
            .Calling(c => c.All())
            .ShouldHave()
            .MemoryCache(cache => cache
            .ContainingEntry(e => e
                .WithKey(AllServicesKey)
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromDays(1))
                .WithValueOfType<List<ServiceViewModel>>()))
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<ServiceViewModel>>()
            .Passing(v => v.Should().HaveCount(10)));
    }
}

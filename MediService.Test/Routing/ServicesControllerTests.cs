using MediService.ASP.NET_Core.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MediService.Test.Routing
{
    public class ServicesControllerTests
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Services/All")
            .WithUser())
            .To<ServicesController>(c => c.All());
    }
}

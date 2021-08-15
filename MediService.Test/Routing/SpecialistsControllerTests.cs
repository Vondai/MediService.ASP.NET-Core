using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using Xunit;

namespace MediService.Test.Routing
{
    public class SpecialistsControllerTests
    {
        [Fact]
        public void AllShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Specialists/All"))
            .To<SpecialistsController>(c => c.All());
    }
}

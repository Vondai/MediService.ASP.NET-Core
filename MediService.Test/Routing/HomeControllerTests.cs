using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using Xunit;

namespace MediService.Test.Routing
{
    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldBeMappedSlash()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/"))
            .To<HomeController>(c => c.Index());

        [Fact]
        public void IndexShouldBeMappedSlashHome()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Home"))
            .To<HomeController>(c => c.Index());

        [Fact]
        public void FaqShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Home/Faq"))
            .To<HomeController>(c => c.Faq());
    }
}

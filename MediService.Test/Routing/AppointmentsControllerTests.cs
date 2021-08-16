using Microsoft.AspNetCore.Http;
using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using Xunit;

namespace MediService.Test.Routing
{
    public class AppointmentsControllerTests
    {
        [Fact]
        public void GetMakeShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Make")
                .WithQueryString("?info=test")
                .WithUser())
            .To<AppointmentsController>(c => c.Make("test"))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void PostMakeShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Make")
                .WithQueryString("?info=test")
            .WithMethod(HttpMethod.Post)
                .WithUser())
            .To<AppointmentsController>(c => c.Make("test"))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void GetMineShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Mine")
                .WithUser())
            .To<AppointmentsController>(c => c.Mine())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void DetailsShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Details")
                .WithUser()
                .WithMethod(HttpMethod.Get))
            .To<AppointmentsController>(c => c.Details(With.Value("test")))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void FinishShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Finish")
                .WithUser()
                .WithMethod(HttpMethod.Get))
            .To<AppointmentsController>(c => c.Finish(With.Value("test")))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void CancelShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Cancel")
                .WithUser()
            .WithMethod(HttpMethod.Get))
            .To<AppointmentsController>(c => c.Cancel(With.Value("test")))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void GetArchiveShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Archive")
                .WithUser())
            .To<AppointmentsController>(c => c.Archive())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());
    }
}

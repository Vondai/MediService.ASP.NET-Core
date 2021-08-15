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
                .WithUser())
            .To<AppointmentsController>(c => c.Make())
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
            .WithMethod(HttpMethod.Post)
                .WithUser())
            .To<AppointmentsController>(c => c.Make())
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
        public void PostDetailsShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Details")
                .WithUser()
                .WithMethod(HttpMethod.Post))
            .To<AppointmentsController>(c => c.Details(With.Value("test")))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests()
                .RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void PostFinishShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Finish")
                .WithUser()
                .WithMethod(HttpMethod.Post))
            .To<AppointmentsController>(c => c.Finish(With.Value("test")))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests()
                .RestrictingForHttpMethod(HttpMethod.Post));

        [Fact]
        public void PostCancelShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Appointments/Cancel")
                .WithUser()
                .WithMethod(HttpMethod.Post))
            .To<AppointmentsController>(c => c.Cancel(With.Value("test")))
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests()
                .RestrictingForHttpMethod(HttpMethod.Post));

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

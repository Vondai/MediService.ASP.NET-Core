using MediService.ASP.NET_Core.Controllers;
using MyTested.AspNetCore.Mvc;
using Xunit;

namespace MediService.Test.Routing
{
    public class ReviewsControllerTests
    {
        [Fact]
        public void GetCreateShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Reviews/Create")
            .WithUser())
            .To<ReviewsController>(c => c.Create())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void PostCreateShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Reviews/Create")
            .WithMethod(HttpMethod.Post)
            .WithUser())
            .To<ReviewsController>(c => c
                .Create())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());
    }
}

using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using Xunit;

namespace MediService.Test.Routing
{
    public class SubscriptionsControllerTests
    {
        [Fact]
        public void GetAllShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
            .WithPath("/Subscriptions/All")
            .WithUser())
            .To<SubscriptionsController>(c => c.All());

        [Fact]
        public void GetSubscribeShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Subscriptions/Subscribe")
                .WithUser())
            .To<SubscriptionsController>(c => c.Subscribe())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());

        [Fact]
        public void PostSubscribeShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Subscriptions/Subscribe")
                .WithMethod(HttpMethod.Post)
                .WithUser())
            .To<SubscriptionsController>(c => c
                .Subscribe())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests());
    }
}

using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Models.Messages;
using Xunit;

namespace MediService.Test.Routing
{
    public class MessagesControllerTests
    {
        [Fact]
        public void GetMineShouldBeMapped()
           => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithPath("/Messages/Mine")
                    .WithUser())
                .To<MessagesController>(c => c
                    .Mine())
                .Which()
                .ShouldReturn()
                .View();

        [Fact]
        public void GetSendShouldBeMapped()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                    .WithUser()
                    .WithPath("/Messages/Send")
                    .WithQueryString("?id=test"))
                .To<MessagesController>(c => c
                    .Send("test"));

        [Fact]
        public void PostSendShouldBeMapped()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithUser()
                .WithPath("/Messages/Send")
                .WithQueryString("?id=test")
                .WithMethod(HttpMethod.Post))
                .To<MessagesController>(c => c
                    .Send("test", new MessageFormModel()));

        [Fact]
        public void GetDetailsShouldBeMapped()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithUser()
                .WithPath("/Messages/Details")
                .WithQueryString("?id=test")
                .WithMethod(HttpMethod.Get))
                .To<MessagesController>(c => c
                    .Details("test"));

        [Fact]
        public void GetDeleteShouldBeMapped()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithUser()
                .WithPath("/Messages/Delete")
                .WithQueryString("?id=test")
                .WithMethod(HttpMethod.Get))
                .To<MessagesController>(c => c
                    .Delete("test"));

        [Fact]
        public void GetCountNewShouldBeMapped()
            => MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithUser()
                .WithPath("/Messages/GetCount")
                .WithMethod(HttpMethod.Get))
                .To<MessagesController>(c => c
                    .GetCount());

    }
}

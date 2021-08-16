using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using Xunit;
using MediService.ASP.NET_Core.Models.Users;

namespace MediService.Test.Routing
{
    public class AccountControllerTests
    {
        [Fact]
        public void GetRegisterShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Account/Register")
                .WithMethod(HttpMethod.Get))
            .To<AccountController>(c => c
                .Register())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
            .AllowingAnonymousRequests());

        [Fact]
        public void PostRegisterShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Account/Register")
                .WithMethod(HttpMethod.Post)
                .WithFormFields(new UserRegisterFormModel()))
            .To<AccountController>(c => c
                .Register())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
            .AllowingAnonymousRequests());

        [Fact]
        public void GetLoginShouldBeMapped()
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Account/Login")
                .WithMethod(HttpMethod.Get))
            .To<AccountController>(c => c
                .Login())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
            .AllowingAnonymousRequests());

        [Fact]
        public void GetLogoutShouldBeMapped() 
            => MyPipeline
            .Configuration()
            .ShouldMap(request => request
                .WithPath("/Account/Logout")
                .WithUser()
                .WithMethod(HttpMethod.Get))
            .To<AccountController>(c => c
                .Logout())
            .Which()
            .ShouldHave()
            .ActionAttributes(att => att
            .RestrictingForAuthorizedRequests());
    }
}

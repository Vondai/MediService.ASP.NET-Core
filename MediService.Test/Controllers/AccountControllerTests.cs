using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Models.Users;
using Xunit;

using static MediService.Test.Data.Accounts;
using MediService.ASP.NET_Core.Data.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace MediService.Test.Controllers
{
    public class AccountControllerTests
    {
        //GET Register
        [Fact]
        public void GetRegisterShouldReturnView()
            => MyController<AccountController>
            .Instance()
            .WithoutUser()
            .Calling(c => c.Register())
            .ShouldHave()
            .ActionAttributes(att => att
                .AllowingAnonymousRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        //POST Register
        [Fact]
        public void PostRegisterShouldRegisterUserAndRedirect()
        {
            var model = ValidRegisterModel();

            MyController<AccountController>
             .Instance()
             .WithoutUser()
             .Calling(c => c.Register(model))
             .ShouldHave()
             .ActionAttributes(att => att
                 .AllowingAnonymousRequests()
                .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
            .ShouldHave()
            .Data(d => d.WithSet<User>(u => u
                .Any(u => u.UserName == model.Username)))
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Account/Login");
        }

        [Fact]
        public void PostRegisterInvalidCityShouldReturnViewWithError()
        {
            var model = new UserRegisterFormModel();

            MyController<AccountController>
             .Instance()
             .WithoutUser()
             .Calling(c => c.Register(model))
             .ShouldHave()
             .ActionAttributes(att => att
                 .AllowingAnonymousRequests()
                .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
             .ShouldReturn()
             .View()
             .AndAlso()
             .ShouldPassForThe<ViewResult>(vr => Assert.Equal(model, vr.Model));
        }

        [Fact]
        public void PostRegisterDuplicateUsernameShouldReturnViewWithError()
        {
            var model = ValidRegisterModel();

            MyController<AccountController>
             .Instance()
             .WithData(new User() { UserName = "TestUsername", NormalizedUserName = "TESTUSERNAME" })
             .WithoutUser()
             .Calling(c => c.Register(model))
             .ShouldHave()
             .ActionAttributes(att => att
                 .AllowingAnonymousRequests()
                .RestrictingForHttpMethod(HttpMethod.Post))
             .AndAlso()
             .ShouldReturn()
             .View()
             .AndAlso()
             .ShouldPassForThe<ViewResult>(vr => Assert.Equal(model, vr.Model));
        }

        //GET Login
        [Fact]
        public void GetLoginShouldReturnView()
            => MyController<AccountController>
            .Instance()
            .WithoutUser()
            .Calling(c => c.Login())
            .ShouldHave()
            .ActionAttributes(att => att
                .AllowingAnonymousRequests())
            .AndAlso()
            .ShouldReturn()
            .View();

        //POST Login
        [Fact]
        public void PostLoginNullPasswordShouldReturnViewWithModel()
        {
            var model = new UserLoginFormModel() { Username = "test" };
            MyController<AccountController>
            .Instance()
            .WithoutUser()
            .Calling(c => c.Login(model, ""))
            .ShouldHave()
            .ActionAttributes(att => att
                .AllowingAnonymousRequests()
               .RestrictingForHttpMethod(HttpMethod.Post))
            .AndAlso()
            .ShouldReturn()
            .View()
            .AndAlso()
            .ShouldPassForThe<ViewResult>(vr =>
                Assert.Equal(model, vr.Model));
        }
    }
}

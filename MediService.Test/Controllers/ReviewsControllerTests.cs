using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Reviews;
using Xunit;

using static MediService.Test.Data.Reviews;

namespace MediService.Test.Controllers
{
    public class ReviewsControllerTests
    {
        [Fact]
        public void GetCreateWithUserShouldReturnViewWithCorrectModel()
            => MyController<ReviewsController>
            .Instance(i => i
            .WithUser(TestUser.Identifier))
            .Calling(c => c.Create())
            .ShouldHave()
            .ActionAttributes(att => att.RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<CreateReviewFormModel>());

        [Fact]
        public void GetCreateWithUserWithReviewShouldRedirectWithError()
            => MyController<ReviewsController>
            .Instance()
            .WithUser()
            .WithData(UserWithReview())
            .Calling(c => c.Create())
            .ShouldHave()
            .ActionAttributes(att => att.RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldHave()
            .TempData(td => td.ContainingEntryWithKey("Error"))
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Home");

        [Theory]
        [InlineData("TestReviewTitle", "TestReviewDescription", 2)]
        public void PostCreateShouldWorkCorrectly
            (string title, string description, int rating)
        {
            var model = new CreateReviewFormModel
            {
                Title = title,
                Description = description,
                Rating = rating,
            };

            MyController<ReviewsController>
            .Instance()
            .WithUser()
            .Calling(c => c.Create(model))
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .ValidModelState()
            .Data(d => d
                .WithSet<Review>(review => review
                .Any(r =>
                r.Title == title &&
                r.Description == description &&
                r.Rating == rating &&
                r.UserId == TestUser.Identifier)))
            .AndAlso()
            .ShouldHave()
            .TempData(td => td
                .ContainingEntryWithKey("Success"))
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Home");
        }


        [Theory]
        [InlineData("TestReviewTitle", "TestReviewDescription", 10)]
        public void PostCreateWithInvalidRatingShouldReturnViewWithError
            (string title, string description, int rating)
        {
            var model = new CreateReviewFormModel()
            {
                Title = title,
                Description = description,
                Rating = rating
            };

            MyController<ReviewsController>
            .Instance()
            .WithUser()
            .Calling(c => c.Create(model))
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .InvalidModelState(withNumberOfErrors: 1)
            .AndAlso()
            .ShouldReturn()
            .View()
            .AndAlso()
            .ShouldPassForThe<ViewResult>(vr =>
                Assert.Same(model, vr.Model));
        }

        [Theory]
        [InlineData("", "", 2)]
        public void PostCreateWithInvalidModelShouldReturnViewWithError
            (string title, string description, int rating)
        {
            var model = new CreateReviewFormModel()
            {
                Title = title,
                Description = description,
                Rating = rating
            };

            MyController<ReviewsController>
            .Instance()
            .WithUser()
            .Calling(c => c.Create(model))
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .InvalidModelState(withNumberOfErrors: 4)
            .AndAlso()
            .ShouldReturn()
            .View()
            .AndAlso()
            .ShouldPassForThe<ViewResult>(vr =>
                Assert.Same(model, vr.Model));
        }
    }
}

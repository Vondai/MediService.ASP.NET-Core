using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Models.Subscriptions;
using MediService.ASP.NET_Core.Data.Models;
using FluentAssertions;
using Xunit;

using static MediService.Test.Data.Subscriptions;
using static MediService.ASP.NET_Core.WebConstants.Cache;
using static MediService.ASP.NET_Core.Areas.Admin.AdminConstants;
using static MediService.Test.Data.Accounts;
using static MediService.ASP.NET_Core.WebConstants.GlobalMessage;

namespace MediService.Test.Controllers
{
    public class SubscriptionsControllerTests
    {
        [Fact]
        public void AllShouldReturnViewWithAllSubscriptions()
            => MyController<SubscriptionsController>
            .Instance(i => i
            .WithData(ThreeSubscriptions()))
            .Calling(c => c.All())
            .ShouldHave()
            .MemoryCache(cache => cache
            .ContainingEntry(e => e
                .WithKey(AllSubscriptionsKey)
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromDays(1))
                .WithValueOfType<List<SubscriptionViewModel>>()))
            .AndAlso()
            .ShouldHave()
            .ActionAttributes(att => att
                .AllowingAnonymousRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<List<SubscriptionViewModel>>()
                .Passing(v => v.Should().HaveCount(3)));

        [Fact]
        public void GetSubscribeShouldWorkCorrectly()
            => MyController<SubscriptionsController>
            .Instance()
            .Calling(c => c.Subscribe())
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
            .WithModel(SubscriptionFormModelTest()));

        [Fact]
        public void GetSubscribeWithAdminShouldRedirectWithError()
            => MyController<SubscriptionsController>
            .Instance()
            .WithUser(TestUser.Username, new[] { AdminRoleName })
            .Calling(c => c.Subscribe())
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldHave()
            .TempData(withNumberOfEntries: 1)
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Home");

        [Fact]
        public void GetSubscribeWithSpecialistShouldRedirectWithError()
            => MyController<SubscriptionsController>
            .Instance()
            .WithData(UserSpecilist())
            .WithUser()
            .Calling(c => c.Subscribe())
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldHave()
            .TempData(withNumberOfEntries: 1)
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Home");

        [Fact]
        public void GetSubscribeWithActiveAppointmentShouldRedirectWithError()
            => MyController<SubscriptionsController>
            .Instance()
            .WithData(UserWithActiveAppointment())
            .WithUser()
            .Calling(c => c.Subscribe())
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldHave()
            .TempData(withNumberOfEntries: 1)
            .AndAlso()
            .ShouldReturn()
            .Redirect(r => r
                .To<AppointmentsController>(c => c.Mine()));

        [Fact]
        public void PostSubscribeShouldWorkCorrectly()
        {
            MyController<SubscriptionsController>
            .Instance()
            .WithData(ThreeSubscriptions())
            .WithData(ValidUser())
            .WithUser()
            .Calling(c => c.Subscribe(ValidSubscribeFormModel()))
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldHave()
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .Data(d => d
                .WithSet<Subscription>(sub => sub
                    .Any(s => s
                    .Users.Any(u => u.Id == TestUser.Identifier))))
            .AndAlso()
            .ShouldHave()
            .TempData(td => td
                .ContainingEntryWithKey(SuccessKey))
            .AndAlso()
            .ShouldReturn()
            .Redirect(r => r
                .To<AppointmentsController>(c => c.Make()));
        }

        [Fact]
        public void PostSubscribeWithInvalidModelShouldReturnViewWithError()
        {
            var model = InvalidCardAndCvc();

            MyController<SubscriptionsController>
            .Instance()
            .WithData(ThreeSubscriptions())
            .WithUser()
            .Calling(c => c.Subscribe(model))
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .InvalidModelState(withNumberOfErrors: 2)
            .AndAlso()
            .ShouldReturn()
            .View()
            .AndAlso()
            .ShouldPassForThe<ViewResult>(vr =>
            Assert.Same(model, vr.Model));
        }

        [Fact]
        public void PostSubscribeWithInvalidSubscriptionShouldReturnViewWithError()
        {
            var model = InvalidSubscription();

            MyController<SubscriptionsController>
            .Instance()
            .WithData(ThreeSubscriptions())
            .WithUser()
            .Calling(c => c.Subscribe(model))
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
    }
}

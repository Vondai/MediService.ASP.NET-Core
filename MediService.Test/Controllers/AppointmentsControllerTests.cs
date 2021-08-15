using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Appointments;
using Microsoft.AspNetCore.Mvc;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using static MediService.Test.Data.Accounts;
using static MediService.Test.Data.Appointments;
using static MediService.Test.Data.Services;

namespace MediService.Test.Controllers
{
    public class AppointmentsControllerTests
    {
        //GET Make
        [Fact]
        public void GetMakeShouldWorkCorrectly()
            => MyController<AppointmentsController>
            .Instance()
            .WithData(UserWithSubscription())
            .WithUser()
            .Calling(c => c.Make())
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View(view => view
            .WithModel(AppointmentFormModelTest()));

        [Fact]
        public void GetMakeWithSpecialistShouldRedirectWithError()
            => MyController<AppointmentsController>
            .Instance()
            .WithData(UserSpecilist())
            .WithUser()
            .Calling(c => c.Make())
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
        public void GetMakeWithNonSubscriberShouldRedirectWithError()
            => MyController<AppointmentsController>
            .Instance()
            .WithUser()
            .Calling(c => c.Make())
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldHave()
            .TempData(withNumberOfEntries: 1)
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Subscriptions/All");

        [Fact]
        public void GetMakeWithSubscriberAndNoAvailableAppointmentsShouldRedirectWithError()
            => MyController<AppointmentsController>
            .Instance()
            .WithData(UserWithSubscriptionAndAppointments())
            .WithUser()
            .Calling(c => c.Make())
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldHave()
            .TempData(withNumberOfEntries: 1)
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Appointments/Mine");

        //POST Make
        [Fact]
        public void PostMakeShouldWorkCorrectly()
        {
            var testUser = ValidUser();
            testUser.Addresses.Add(new Address { FullAddress = "15 Test str." });
            var testSpecialist = new Specialist();
            var testDate = DateTime.Now.AddDays(10);
            var testDateDb = testDate.ToUniversalTime();
            testSpecialist.Services.Add(new Service() { Id = 1 });
            var testModel = new AppointmentFormModel()
            {
                Address = testUser.Addresses.Select(x => x.FullAddress).FirstOrDefault(),
                ServiceId = 1,
                Date = testDate.ToString()
            };

            MyController<AppointmentsController>
            .Instance()
            .WithData(testSpecialist)
            .WithData(testUser)
            .WithUser()
            .Calling(c => c.Make(testModel))
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
                .WithSet<Appointment>(app => app
                    .Any(a =>
                    a.UserId == testUser.Id &&
                    a.ServiceId == testModel.ServiceId &&
                    a.SpecialistId == testSpecialist.Id &&
                    a.Date.Day == testDateDb.Day &&
                    a.Date.Hour == testDateDb.Hour &&
                    a.Date.Minute == testDateDb.Minute)))
            .AndAlso()
            .ShouldHave()
            .TempData(td => td
                .ContainingEntryWithKey("Success"))
            .AndAlso()
            .ShouldReturn()
            .Redirect(r => r
                .To<AppointmentsController>(c => c.Mine()));
        }

        [Fact]
        public void PostMakeWithInvalidModelShouldReturnViewWithError()
        {
            var model = InvalidModel();

            MyController<AppointmentsController>
            .Instance()
            .WithUser()
            .Calling(c => c.Make(model))
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

        [Fact]
        public void PostMakeWithInvaidAddressShouldReturnViewWithError()
        {
            var model = InvalidModel();
            model.Date = DateTime.Now.AddDays(10).ToString();
            model.ServiceId = 1;
            var testUser = UserWithSubscription();
            testUser.Addresses.Add(new Address() { FullAddress = "TestAddress" });

            MyController<AppointmentsController>
            .Instance()
            .WithData(TenServices)
            .WithData(testUser)
            .WithUser()
            .Calling(c => c.Make(model))
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

        [Fact]
        public void PostMakeWithInvalidServiceShouldReturnViewWithError()
        {
            var model = InvalidModel();
            model.Address = "TestAddress";
            model.Date = DateTime.Now.AddDays(10).ToString();
            model.ServiceId = 100;
            var testUser = UserWithSubscription();
            testUser.Addresses.Add(new Address() { FullAddress = "TestAddress" });

            MyController<AppointmentsController>
            .Instance()
            .WithData(TenServices)
            .WithData(testUser)
            .WithUser()
            .Calling(c => c.Make(model))
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

        [Fact]
        public void PostMakeWithPastDateShouldReturnViewWithError()
        {
            var model = InvalidModel();
            model.Address = "TestAddress";
            model.Date = DateTime.Now.AddDays(-10).ToString();
            model.ServiceId = 1;
            var testUser = UserWithSubscription();
            testUser.Addresses.Add(new Address() { FullAddress = "TestAddress" });

            MyController<AppointmentsController>
            .Instance()
            .WithData(TenServices)
            .WithData(testUser)
            .WithUser()
            .Calling(c => c.Make(model))
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

        [Fact]
        public void PostMakeWithNoSpecialistShouldRedirectWithError()
        {
            var testSpecialist = new Specialist();
            var model = InvalidModel();
            model.Address = "TestAddress";
            model.Date = DateTime.Now.AddDays(10).ToString();
            model.ServiceId = 1;
            var testUser = UserWithSubscription();
            testUser.Addresses.Add(new Address() { FullAddress = "TestAddress" });

            MyController<AppointmentsController>
            .Instance()
            .WithData(TenServices)
            .WithData(testUser)
            .WithUser()
            .Calling(c => c.Make(model))
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForHttpMethod(HttpMethod.Post)
                .RestrictingForAuthorizedRequests())
            .ValidModelState()
            .AndAlso()
            .ShouldHave()
            .TempData(td => td
                .ContainingEntryWithKey("Error"))
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Home");
        }

        [Fact]
        public void MineWithInvalidIdShouldWorkReturnNotFound()
        {
            var userSpecialist = UserSpecilist();
            var appId = "";
            var appointments = new List<Appointment>();
            for (int i = 0; i < 10; i++)
            {
                appointments.Add(new Appointment() { Id = $"TestId{i}" });
            }

            MyController<AppointmentsController>
                .Instance(i => i
                    .WithData(userSpecialist)
                    .WithData(appointments)
                    .WithUser(userSpecialist.UserId))
                .Calling(c => c.Details(appId))
                .ShouldHave()
                .ActionAttributes(att => att
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void MineShouldReturnViewWithModel()
        {
            var user = ValidUser();
            var appId = "testAppointmentId";
            user.Appointments.Add(new Appointment() { Id = appId, IsCanceled = false, IsDone = false });

            MyController<AppointmentsController>
           .Instance()
           .WithData(user)
           .WithUser()
           .Calling(a => a.Mine())
           .ShouldHave()
           .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
           .AndAlso()
           .ShouldReturn()
           .View(v => v
            .WithModelOfType<ICollection<AppointmentViewModel>>());
        }

        [Fact]
        public void MineWithNonSpecialistShouldReturnNotFound()
            => MyController<AppointmentsController>
            .Instance()
            .WithUser()
            .Calling(c => c.Details("testId"))
            .ShouldHave()
            .ActionAttributes(att => att
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .NotFound();

        //POST Finish

        [Fact]
        public void FinishShouldWorkCorrectly()
        {
            var appId = "testAppointmentId";
            var appointment = new Appointment() { Id = appId };
            var userSpecialist = UserSpecilist();

            MyController<AppointmentsController>
                .Instance()
                .WithData(appointment)
                .WithData(userSpecialist)
                .WithUser()
                .Calling(c => c.Finish(appId))
                .ShouldHave()
                .ActionAttributes(att => att
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldHave()
                .Data(d => d
                    .WithSet<Appointment>(s => s
                        .Any(a => a.IsDone == true)))
                .AndAlso()
                .ShouldHave()
                .TempData(td => td
                    .ContainingEntryWithKey("Success"))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Appointments/Mine");
        }
        [Fact]
        public void FinishWithNonSpecialistShouldReturnNotFound()
        {
            var appId = "testAppointmentId";

            MyController<AppointmentsController>
                .Instance()
                .WithUser()
                .Calling(c => c.Finish(appId))
                .ShouldHave()
                .ActionAttributes(att => att
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void FinishWithInvalidAppointmentShouldRedirectWithError()
        {
            var userSpecialist = UserSpecilist();
            var appointment = new Appointment() { Id = "testAppointmentId" };

            MyController<AppointmentsController>
                .Instance()
                .WithData(userSpecialist)
                .WithUser()
                .Calling(c => c.Finish("InvaliId"))
                .ShouldHave()
                .ActionAttributes(att => att
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldHave()
                .TempData(td => td
                    .ContainingEntryWithKey("Error"))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Home");
        }

        [Fact]
        public void CancelShouldWorkCorrectly()
        {
            var user = ValidUser();
            var appId = "testAppointmentId";
            var appointment = new Appointment() { Id = appId, Date = DateTime.Now.AddDays(10) };

            MyController<AppointmentsController>
                .Instance()
                .WithData(appointment)
                .WithData(user)
                .Calling(c => c.Cancel(appId))
                .ShouldHave()
                .ActionAttributes(att => att
                    .RestrictingForAuthorizedRequests()
                    .RestrictingForHttpMethod(HttpMethod.Post))
                .AndAlso()
                .ShouldHave()
                .TempData(td => td
                    .ContainingEntryWithKey("Success"))
                .AndAlso()
                .ShouldHave()
                .Data(d => d
                    .WithSet<Appointment>(s => s
                        .Any(a => a.IsCanceled == true)))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Appointments/Mine");
        }

        [Fact]
        public void CancelhWithSpecialistShouldReturnBadRequest()
        {
            var appId = "testAppointmentId";
            var appointment = new Appointment() { Id = appId };
            var userSpecialist = UserSpecilist();

            MyController<AppointmentsController>
                .Instance()
                .WithData(appointment)
                .WithData(userSpecialist)
                .WithUser()
                .Calling(c => c.Cancel(appId))
                .ShouldReturn()
                .BadRequest();
        }

        [Fact]
        public void CancelWithAnAppointmentDueInAnHourShouldRedirectWithError()
        {
            var appId = "testAppointmentId";
            var appointment = new Appointment() { Id = appId, Date = DateTime.Now.AddMinutes(60) };
            var user = ValidUser();

            MyController<AppointmentsController>
                .Instance()
                .WithData(appointment)
                .WithData(user)
                .WithUser()
                .Calling(c => c.Cancel(appId))
                .ShouldHave()
                .TempData(td => td
                    .ContainingEntryWithKey("Error"))
                .AndAlso()
                .ShouldHave()
                .Data(d => d
                    .WithSet<Appointment>(s => s
                        .Any(a => a.IsCanceled == false)))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Home");
        }

        //GET Archive
        [Fact]
        public void ArchiveShouldWorkCorrectly()
        {
            var user = ValidUser();
            var appointments = new List<Appointment>()
            {
                new Appointment() {IsCanceled = true, UserId = user.Id},
                new Appointment() {IsCanceled = true, UserId = user.Id},
                new Appointment() {IsCanceled = true, UserId = user.Id},
                new Appointment() {IsDone = true, UserId = user.Id},
                new Appointment() {IsDone = true, UserId = user.Id},
            };

            MyController<AppointmentsController>
                .Instance()
                .WithData(user)
                .WithData(appointments)
                .WithUser()
                .Calling(c => c.Archive())
                .ShouldHave()
                .ActionAttributes(att => att
                    .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View(v => v
                    .WithModelOfType<List<AppointmentArchiveViewModel>>());
        }
    }
}

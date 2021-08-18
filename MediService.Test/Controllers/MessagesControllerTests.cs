using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyTested.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Controllers;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Messages;
using Xunit;

using static MediService.Test.Data.Accounts;
using static MediService.ASP.NET_Core.WebConstants.GlobalMessage;

namespace MediService.Test.Controllers
{
    public class MessagesControllerTests
    {
        //GET Send
        [Fact]
        public void GetSendShouldReturnView()
        {
            var validUser = ValidUser();
            var appointment = new Appointment()
            {
                UserId = validUser.Id,
            };

            MyController<MessagesController>
                .Instance()
                .WithData(appointment)
                .WithUser(validUser.Id)
                .Calling(c => c.Send(appointment.Id))
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void GetSendWithInvalidAppointmentShouldReturnNotFound()
        {
            var validUser = ValidUser();
            var appointment = new Appointment()
            {
                UserId = validUser.Id,
            };

            MyController<MessagesController>
                .Instance()
                .WithData(appointment)
                .WithUser(validUser.Id)
                .Calling(c => c.Send("invalid"))
                .ShouldReturn()
                .NotFound();
        }

        //POST Send

        [Fact]
        public void PostSendShouldWorkCorrectlyAndRedirect()
        {
            var validUser = new User()
            {
                FullName = "User Full Name",
            };
            var specialistUser = new Specialist()
            {
                User = new User()
                {
                    FullName = "Specialist Full Name",
                }
            };
            var appointment = new Appointment()
            {
                User = validUser,
                Specialist = specialistUser,
            };

            var messageForm = new MessageFormModel()
            {
                Title = "test1",
                Content = "Some long test"
            };

            MyController<MessagesController>
                .Instance()
                .WithData(appointment)
                .WithUser(validUser.Id)
                .Calling(c => c.Send(appointment.Id, messageForm))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(d => d
                    .WithSet<Message>(ds => ds
                        .Any(m => m
                            .RecipientId == specialistUser.UserId)))
                .AndAlso()
                .ShouldHave()
                .TempData(td => td
                    .ContainingEntryWithKey(SuccessKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Appointments/Mine");
        }

        [Fact]
        public void PostSendWithInvalidModelShouldReturnViewWithModel()
        {
            var messageForm = new MessageFormModel()
            {
                Title = "",
                Content = ""
            };

            MyController<MessagesController>
                .Instance()
                .WithUser()
                .Calling(c => c.Send("testAppId", messageForm))
                .ShouldHave()
                .InvalidModelState()
                .AndAlso()
                .ShouldReturn()
                .View()
                .ShouldPassForThe<ViewResult>(vr => Assert.Same(messageForm, vr.Model));
        }

        [Fact]
        public void PostSendWithInvalidAppointmentShouldReturnNotFound()
        {
            var validModel = new MessageFormModel()
            {
                Title = "Title test",
                Content = "Content test"
            };

            MyController<MessagesController>
                .Instance()
                .WithUser()
                .Calling(c => c.Send("testAppId", validModel))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void PostSendWithSpecialistShouldWorkCorrectlyAndRedirect()
        {
            var validUser = new User()
            {
                FullName = "User Full Name",
            };
            var specialistUser = new Specialist()
            {
                User = new User()
                {
                    Id = "SpecialistUserId",
                    FullName = "Specialist Full Name",
                    UserName = "TestUsername"
                }
            };
            var appointment = new Appointment()
            {
                User = validUser,
                Specialist = specialistUser,
            };

            var messageForm = new MessageFormModel()
            {
                Title = "test1",
                Content = "Some long test"
            };

            MyController<MessagesController>
                .Instance()
                .WithData(specialistUser)
                .WithUser(specialistUser.UserId, specialistUser.User.UserName)
                .WithData(appointment)
                .Calling(c => c.Send(appointment.Id, messageForm))
                .ShouldHave()
                .ValidModelState()
                .AndAlso()
                .ShouldHave()
                .Data(d => d
                    .WithSet<Message>(ds => ds
                        .Any(m => m
                            .RecipientId == validUser.Id)))
                .AndAlso()
                .ShouldHave()
                .TempData(td => td
                    .ContainingEntryWithKey(SuccessKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Appointments/Mine");
        }

        //GET Details
        [Fact]
        public void GetDetailsShouldWorkCorrectly()
        {
            var message = new Message()
            {
                Id = "TestMessageId",
                Recipient = new User()
                {
                    UserName = "TestRecipient",
                },
                Sender = "SenderTest"
            };

            MyController<MessagesController>
                .Instance()
                .WithData(message)
                .WithUser(message.Recipient.Id, message.Recipient.UserName)
                .Calling(c => c.Details(message.Id))
                .ShouldHave()
                .Data(d => d
                    .WithSet<Message>(ds => ds.FirstOrDefault(m => m
                        .Id == message.Id
                        && m.IsSeen == true)))
                .AndAlso()
                .ShouldReturn()
                .View(v => v
                    .WithModelOfType<MessageDetailsViewModel>());
        }

        [Fact]
        public void GetDetailsWithInvalidMessageShouldReturnNotFound()
        {
            MyController<MessagesController>
                .Instance()
                .WithUser()
                .Calling(c => c.Details("Invalid"))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void GetDeleteShouldWorkCorrectlyAndRedirect()
        {
            var message = new Message()
            {
                Id = "TestMessageId",
            };

            MyController<MessagesController>
                .Instance()
                .WithData(message)
                .WithUser()
                .Calling(c => c.Delete(message.Id))
                .ShouldHave()
                .TempData(dt => dt
                    .ContainingEntryWithKey(SuccessKey))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Messages/Mine");
        }

        [Fact]
        public void GetDeleteWithInvalidMessageShouldReturnNotFound()
        {
            MyController<MessagesController>
                .Instance()
                .WithUser()
                .Calling(c => c.Delete("invalid"))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void GetCountShouldWorkCorrectly()
        {
            var user = new User()
            {
                Id = "TestUserId",
                UserName = "TestUsername",
            };
            var message = new Message()
            {
                Recipient = user,
                IsSeen = false
            };
            MyController<MessagesController>
                .Instance()
                .WithData(message)
                .WithUser(user.Id, user.UserName)
                .Calling(c => c.GetCount())
                .ShouldReturn()
                .ResultOfType<int>(rt => rt.Equals(1));
        }
    }
}

﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Models.Appointments;
using MediService.ASP.NET_Core.Services.Accounts;
using MediService.ASP.NET_Core.Services.Appointments;
using MediService.ASP.NET_Core.Services.MedicalServices;
using MediService.ASP.NET_Core.Services.Specialists;
using MediService.ASP.NET_Core.Services.Subscriptions;

namespace MediService.ASP.NET_Core.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ISpecialistService specialists;
        private readonly ISubscriptionService subscriptions;
        private readonly IAppointmentService appointments;
        private readonly IAccountService accounts;
        private readonly IMedicalService medicalService;

        public AppointmentsController
            (ISpecialistService specialists,
            ISubscriptionService subscriptions,
            IAppointmentService appointments,
            IAccountService accounts, IMedicalService medicalService)
        {
            this.specialists = specialists;
            this.subscriptions = subscriptions;
            this.appointments = appointments;
            this.accounts = accounts;
            this.medicalService = medicalService;
        }

        public IActionResult Make()
        {
            var userId = this.User.Id();

            //Cannot make appointments if user is a specialist.
            var isSpecialist = specialists.IsSpecialist(userId);
            if (isSpecialist)
            {
                this.TempData.Add("Error", "Only non-specialists can make appointments.");
                return Redirect("/Home");
            }
            //Cannot make appointments if user is not a subscriber.
            var isSubscriber = this.subscriptions.IsSubscriber(userId);
            if (!isSubscriber)
            {
                this.TempData.Add("Error", "Only subscribers can make appointments.");
                return Redirect("/Subscriptions/All");
            }
            //Cannot make an appointment if user has reached the maximum count of available appointments.
            var userAppointmentCount = this.appointments.GetUserAppointmetsCount(userId);
            var subscriptionAppointmentCount = this.subscriptions.GetSubscriptionAppointmentCount(userId);
            if (userAppointmentCount == subscriptionAppointmentCount)
            {
                this.TempData.Add("Error", "You have reached the maximum number of appointments for the current subscription plan.");
                return Redirect("/Appointments/Mine");
            }
            //Appointments left for the current subscription plan
            var userAppointmentsLeft = subscriptionAppointmentCount - userAppointmentCount;
            return View(new AppointmentFormModel()
            {
                Address = this.accounts.GetAddress(userId),
                Services = this.medicalService.GetServices(),
                AppointmentsLeft = userAppointmentsLeft
            });
        }

        [HttpPost]
        public async Task<IActionResult> Make(AppointmentFormModel model)
        {
            var userId = this.User.Id();
            if (!ModelState.IsValid)
            {
                model.Services = this.medicalService.GetServices();
                return View(model);
            }
            //Check for account address
            if (model.Address != this.accounts.GetAddress(userId))
            {
                ModelState.AddModelError(string.Empty, "Invalid Address.");
            }
            //Check for invalid medical service
            var isValidService = this.medicalService.IsValidService(model.ServiceId);
            if (!isValidService)
            {
                ModelState.AddModelError(string.Empty, "Invalid service.");
            }
            //Check for invalid date format
            var isValidDate = DateTime.TryParse(model.Date, out DateTime date);
            if (!isValidDate)
            {
                ModelState.AddModelError(string.Empty, "Invalid date.");
            }
            //Check for invalid date input
            var canMakeAppointmentFromDate = this.appointments.CanMakeAppointmentFromDate(date);
            if (!canMakeAppointmentFromDate)
            {
                ModelState.AddModelError
                    (string.Empty, $"Date must be between {DateTime.Now:MM-dd-yyyy HH:mm} and {DateTime.Now.AddMonths(1):MM-dd-yyyy HH:mm}.");
            }
            if (ModelState.ErrorCount > 0)
            {
                model.Services = this.medicalService.GetServices();
                var userAppointmentCount = this.appointments.GetUserAppointmetsCount(userId);
                var subscriptionAppointmentCount = this.subscriptions.GetSubscriptionAppointmentCount(userId);
                model.AppointmentsLeft = subscriptionAppointmentCount - userAppointmentCount;
                return View(model);
            }
            var specialistId = this.specialists.GetIdFromService(model.ServiceId);
            if (specialistId == null)
            {
                TempData.Add("Error", "Service has no available specialists. Sorry for the inconvenience.");
                return Redirect("/Home");
            }
            await this.appointments.CreateAppointment
                (model.AdditionalInfo,
                model.ServiceId,
                specialistId,
                date,
                userId);

            TempData.Add("Success", "Successful appointment.");
            return Redirect("/Appointments/Mine");
        }

        public IActionResult Mine()
        {
            var userId = this.User.Id();
            var specialistId = this.specialists.IdByUser(userId);
            var appointments = this.appointments.GetUserAppointments(userId, specialistId);

            return View(appointments);
        }

        public IActionResult Details(string id)
        {
            var isSpecialist = this.specialists.IsSpecialist(User.Id());
            if (!isSpecialist)
            {
                return NotFound();
            }
            //Get appointment details by id
            var appointment = this.appointments.GetAppointmentDetails(id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        public async Task<IActionResult> Finish(string id)
        {
            var isSpecialist = this.specialists.IsSpecialist(this.User.Id());
            if (!isSpecialist)
            {
                return NotFound();
            }

            var isFinished = await this.appointments.FinishAppointment(id);
            if (!isFinished)
            {
                TempData.Add("Error", "An error occurred while processing your request.");
                return Redirect("/Home");
            }
            TempData.Add("Success", "Successfuly archived appointment.");
            return Redirect("/Appointments/Mine");
        }

        public async Task<IActionResult> Cancel(string id)
        {
            var isSpecialist = this.specialists.IsSpecialist(this.User.Id());
            if (isSpecialist)
            {
                return NotFound();
            }
            var isCanceled = await this.appointments.CancelAppointment(id);
            if (!isCanceled)
            {
                TempData.Add("Error", "An error occurred while processing your request.");
                return Redirect("/Home");
            }
            TempData.Add("Success", "Successfuly canceled appointment.");
            return Redirect("/Appointments/Mine");
        }

        public IActionResult Archive()
        {
            var userId = this.User.Id();
            var specialistId = this.specialists.IdByUser(userId);
            var archivedAppointments = this.appointments.GetArchivedAppointments(userId, specialistId);

            return View(archivedAppointments);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Models.Appointments;
using MediService.ASP.NET_Core.Models.Services;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Services.Specialists;
using MediService.ASP.NET_Core.Services.Subscriptions;
using MediService.ASP.NET_Core.Services.Appointments;
using MediService.ASP.NET_Core.Services.Accounts;
using MediService.ASP.NET_Core.Services.MedicalServices;
using System.Threading.Tasks;

namespace MediService.ASP.NET_Core.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly MediServiceDbContext data;
        private readonly ISpecialistService specialists;
        private readonly ISubscriptionService subscriptions;
        private readonly IAppointmentService appointments;
        private readonly IAccountService accounts;
        private readonly IMedicalService medicalService;

        public AppointmentsController
            (MediServiceDbContext data,
            ISpecialistService specialists,
            ISubscriptionService subscriptions,
            IAppointmentService appointments,
            IAccountService accounts, IMedicalService medicalService)
        {
            this.data = data;
            this.specialists = specialists;
            this.subscriptions = subscriptions;
            this.appointments = appointments;
            this.accounts = accounts;
            this.medicalService = medicalService;
        }

        public async Task<IActionResult> Make()
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
            //Archive appointments
            await this.appointments.ArchiveAppointments(userId);
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
            await this.appointments.CreateAppointment
                (model.AdditionalInfo,
                model.ServiceId,
                specialistId,
                date,
                userId);

            TempData.Add("Success", "Successful appointment.");
            return Redirect("/Appointments/Mine");
        }

        public async Task<IActionResult> Mine()
        {
            var userId = this.User.Id();
            var specialistId = this.specialists.IdByUser(userId);
            //Archive appointments
            await this.appointments.ArchiveAppointments(userId, specialistId);
            var appointments = this.data
                .Appointments
                .Where(x => (specialistId != null ? x.SpecialistId == specialistId : x.UserId == userId)
                       && x.IsDone == false
                       && x.IsCanceled == false)
                .OrderBy(a => a.Date)
                .Select(x => new AppointmentViewModel()
                {
                    Id = x.Id,
                    Date = x.Date.ToLocalTime().ToString("MM-dd-yyyy"),
                    Time = x.Date.ToLocalTime().ToString("HH:mm"),
                    ServiceName = this.data.Services.Where(s => s.Id == x.ServiceId).Select(x => x.Name).FirstOrDefault(),
                    Name = specialistId != null ?
                                x.User.FullName :
                                x.Specialist.User.FullName
                })
                .ToList();

            return View(appointments);
        }

        public IActionResult Details(string id)
        {
            var isSpecialist = this.specialists.IsSpecialist(User.Id());
            if (!isSpecialist)
            {
                return NotFound();
            }
            var appointment = this.data
                .Appointments
                .Where(a => a.Id == id)
                .Select(x => new AppointmentDetailsViewModel()
                {
                    Id = x.Id,
                    Date = x.Date.ToLocalTime().ToString("MM-dd-yyyy HH:MM"),
                    AdditionalInfo = x.AdditionalInfo,
                    Address = x.User.Addresses
                    .Select(address => address.FullAddress)
                    .FirstOrDefault(),
                    City = x.User.Addresses.
                    Select(address => address.City)
                    .FirstOrDefault(),
                    PatientName = x.User.FullName,
                    Service = x.Service.Name,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber
                })
                .FirstOrDefault();
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        public IActionResult Finish(string id)
        {
            if (!this.specialists.IsSpecialist(User.Id()))
            {
                return NotFound();
            }
            var appointment = this.data
                .Appointments
                .FirstOrDefault(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            appointment.IsDone = true;
            this.data.SaveChanges();
            TempData.Add("Success", "Successfuly archived appointment.");

            return Redirect("/Appointments/Mine");
        }

        public IActionResult Cancel(string id)
        {
            if (this.specialists.IsSpecialist(User.Id()))
            {
                return NotFound();
            }
            var appointment = this.data
                .Appointments
                .FirstOrDefault(x => x.Id == id);
            if (appointment == null)
            {
                return NotFound();
            }
            appointment.IsCanceled = true;
            this.data.SaveChanges();

            return Redirect("/Appointments/Mine");
        }

        public IActionResult Archive()
        {
            var userId = this.User.Id();
            var specialistId = this.specialists.IdByUser(userId);

            var pastAppointments = this.data
                .Appointments
                .Where(x => (specialistId != null ? x.SpecialistId == specialistId : x.UserId == userId)
                       && (x.IsDone == true
                       || x.IsCanceled == true))
                .OrderBy(x => x.Date)
                .Select(x => new AppointmentPastViewModel()
                {
                    Date = x.Date.ToLocalTime().ToString("MM-dd-yyyy"),
                    ServiceName = x.Service.Name,
                    Status = x.IsDone ? "Finished" : "Canceled"
                })
                .ToList();

            return View(pastAppointments);
        }
    }
}

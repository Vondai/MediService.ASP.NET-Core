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

namespace MediService.ASP.NET_Core.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly MediServiceDbContext data;
        private readonly ISpecialistService specialists;

        public AppointmentsController
            (MediServiceDbContext data,
            ISpecialistService specialists)
        {
            this.data = data;
            this.specialists = specialists;
        }

        public IActionResult Make()
        {
            var userId = this.User.Id();
            var isSubscriber = this.data
                .Users
                .Any(u => u.Id == userId && u.SubscriptionId.HasValue);
            var isSpecialist = specialists.IsSpecialist(userId);
            if (isSpecialist)
            {
                this.TempData.Add("Error", "Only non-specialists can make appointments.");
                return Redirect("/Home");
            }
            if (!isSubscriber)
            {
                this.TempData.Add("Error", "Only subscribers can make appointments.");
                return Redirect("/Subscriptions/All");
            }
            var appointmentCount = this.data
                .Appointments
                .Where(a => a.UserId == userId
                    && a.IsCanceled == false
                    && a.IsDone == false)
                .Count();
            var subscriptionAppointmentCount = this.data
                .Users
                .Where(u => u.Id == userId)
                .Select(x => x.Subscription.AppointmentCount)
                .FirstOrDefault();
            if (appointmentCount == subscriptionAppointmentCount)
            {
                this.TempData.Add("Error", "You have reached the maximum number of appointments for the current subscription plan.");
                return Redirect("/Appointments/Mine");
            }

            return View(new AppointmentFormModel()
            {
                Address = GetUserAddress(),
                Services = GetMediServices(),
                AppointmentsLeft = AppointmentsLeft()
            });
        }

        [HttpPost]
        public IActionResult Make(AppointmentFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(new AppointmentFormModel()
                {
                    Address = GetUserAddress(),
                    Services = GetMediServices(),
                });
            }
            if (model.Address != GetUserAddress())
            {
                ModelState.AddModelError(nameof(model.Address), "Invalid Address.");
                return View(new AppointmentFormModel()
                {
                    Address = GetUserAddress(),
                    Services = GetMediServices(),
                });
            }
            var isValidService = this.data.Services.Any(x => x.Id == model.ServiceId);
            if (!isValidService)
            {
                ModelState.AddModelError(nameof(model.ServiceId), "Invalid service.");
                return View(new AppointmentFormModel()
                {
                    Address = GetUserAddress(),
                    Services = GetMediServices(),
                });
            }
            var isValidDate = DateTime.TryParse(model.Time, out DateTime time);
            if (!isValidDate)
            {
                ModelState.AddModelError(nameof(model.Time), "Invalid date.");
                return View(new AppointmentFormModel()
                {
                    Address = GetUserAddress(),
                    Services = GetMediServices(),
                });
            }
            if (time.ToUniversalTime().Day < DateTime.UtcNow.Day
                && time.ToUniversalTime() > DateTime.UtcNow.AddDays(30))
            {
                ModelState.AddModelError(nameof(model.Time), "Invalid date.");
                return View(new AppointmentFormModel()
                {
                    Address = GetUserAddress(),
                    Services = GetMediServices(),
                });
            }
            var specialistId = this.data.Specialists
                .Where(x => x.Services
                .Any(s => s.Id == model.ServiceId))
                .Select(x => x.Id)
                .FirstOrDefault();
            var appointment = new Appointment()
            {
                AdditionalInfo = model.AdditionalInfo,
                IsCanceled = false,
                IsDone = false,
                ServiceId = model.ServiceId,
                SpecialistId = specialistId,
                Date = time.ToUniversalTime(),
                UserId = this.User.Id(),
            };
            this.data.Appointments.Add(appointment);
            this.data.SaveChanges();

            TempData.Add("Success", "Successful appointment.");
            return Redirect("/Appointments/Mine");
        }

        public IActionResult Mine()
        {
            var userId = this.User.Id();
            var specialistId = this.specialists.IdByUser(userId);

            var appointments = this.data
                .Appointments
                .Where(x => (specialistId != null ? x.SpecialistId == specialistId : x.UserId == userId)
                       && x.IsDone == false
                       && x.IsCanceled == false)
                .OrderBy(a => a.Date)
                .Select(x => new AppointmentViewModel()
                {
                    Id = x.Id,
                    Date = x.Date.ToLocalTime().ToString("dd-MM-yyyy"),
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
                    Date = x.Date.ToLocalTime().ToString("dd-MM-yyyy HH:MM"),
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

        public IActionResult All()
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
                    Date = x.Date.ToLocalTime().ToString("dd-MM-yyyy"),
                    ServiceName = x.Service.Name,
                    Status = x.IsDone ? "Finished" : "Canceled"
                })
                .ToList();

            return View(pastAppointments);
        }

        private int AppointmentsLeft()
        {
            var user = this.data
                .Users
                .FirstOrDefault(u => u.Id == User.Id());
            var appointmentCount = this.data
                .Subscriptions
                .Where(s => s.Users.Contains(user))
                .Select(x => x.AppointmentCount)
                .FirstOrDefault();
            var currAppointmentCount = this.data
                .Appointments
                .Where(a => a.User == user
                    && a.IsCanceled == false
                    && a.IsDone == false)
                .Count();
            return appointmentCount - currAppointmentCount;
        }

        private IEnumerable<ServiceViewFormModel> GetMediServices()
        => this.data.Services
            .Select(x => new ServiceViewFormModel
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToList();

        private string GetUserAddress()
        => this.data.
            Addresses
            .Where(x => x.UserId == this.User.Id())
            .Select(x => x.FullAddress)
            .FirstOrDefault();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Models.Appointments;
using MediService.ASP.NET_Core.Models.Services;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Services.Specialists;

namespace MediService.ASP.NET_Core.Controllers
{
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
            if (!isSubscriber)
            {
                return Redirect("/Subscriptions/All");
            }

            return View(new AppointmentFormModel()
            {
                Address = GetUserAddress(),
                Services = GetMediServices(),
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
                Time = time.ToUniversalTime(),
                UserId = this.User.Id(),
            };
            this.data.Appointments.Add(appointment);
            this.data.SaveChanges();

            return Redirect("/Home");
        }

        public IActionResult Mine()
        {
            var userId = this.User.Id();
            var specialistId = this.specialists.IdByUser(userId);

            var appointments = this.data
                .Appointments
                .Where(x => specialistId != null ? x.SpecialistId == specialistId : x.UserId == userId)
                .OrderBy(a => a.Time)
                .Select(x => new AppointmentViewModel()
                {
                    Id = x.Id,
                    IsCanceled = x.IsCanceled ? "Yes" : "No",
                    IsDone = x.IsDone ? "Yes" : "No",
                    Time = x.Time.ToLocalTime().ToString("dd-MM-yyyy HH:MM"),
                    ServiceName = this.data.Services.Where(s => s.Id == x.ServiceId).Select(x => x.Name).FirstOrDefault(),
                    Name = specialistId != null ?
                                x.User.FullName :
                                x.Specialist.User.FullName
                })
                .ToList();

            return View(appointments);
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

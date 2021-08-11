using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Appointments;

namespace MediService.ASP.NET_Core.Services.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly MediServiceDbContext data;

        public AppointmentService(MediServiceDbContext data)
        {
            this.data = data;
        }

        public bool CanMakeAppointmentFromDate(DateTime date)
            => date >= DateTime.Now
                && date <= DateTime.Now.AddMonths(1);

        public int GetUserAppointmetsCount(string userId)
            => this.data
                .Appointments
                .Where(a => a.UserId == userId
                    && a.IsCanceled == false
                    && a.IsDone == false)
                .Count();

        public async Task<string> CreateAppointment
            (string additionalInfo,
            int serviceId,
            string specialistId,
            DateTime date,
            string userId,
            bool isCanceled = false,
            bool isDone = false)
        {
            var appointment = new Appointment()
            {
                AdditionalInfo = additionalInfo,
                ServiceId = serviceId,
                SpecialistId = specialistId,
                Date = date.ToUniversalTime(),
                UserId = userId,
                IsCanceled = isCanceled,
                IsDone = isDone,
            };

            this.data.Appointments.Add(appointment);
            await this.data.SaveChangesAsync();

            return appointment.Id;
        }

        public async Task<int> ArchiveAppointments(string userId, string specialistId = null)
        {
            var dateNowUniversal = DateTime.Now.ToUniversalTime();
            var appointmentsToArchive = this.data.Appointments
                .Where(a =>
                (specialistId != null ? a.SpecialistId == specialistId : a.UserId == userId)
                && dateNowUniversal >= a.Date
                && a.IsDone == false
                && a.IsCanceled == false)
                .ToList();
            if (appointmentsToArchive.Any())
            {
                foreach (var appointment in appointmentsToArchive)
                {
                    appointment.IsDone = true;
                }
                await this.data.SaveChangesAsync();
            }
            return appointmentsToArchive.Count;
        }

        public AppointmentDetailsViewModel GetAppointmentDetails(string appointmentId)
            => this.data
                .Appointments
                .Where(a => a.Id == appointmentId)
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

        public async Task<bool> FinishAppointment(string appointmentId)
        {
            var appointment = this.GetAppointment(appointmentId);
            if (appointment == null)
            {
                return false;
            }
            appointment.IsDone = true;
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelAppointment(string appointmentId)
        {
            var appointment = this.GetAppointment(appointmentId);
            if (appointment == null)
            {
                return false;
            }
            appointment.IsCanceled = true;
            await this.data.SaveChangesAsync();
            return true;
        }

        public ICollection<AppointmentArchiveViewModel> GetArchivedAppointments(string userId, string specialistId = null)
            => this.data
                .Appointments
                .Where(x => (specialistId != null ? x.SpecialistId == specialistId : x.UserId == userId)
                       && (x.IsDone == true
                       || x.IsCanceled == true))
                .OrderBy(x => x.Date)
                .Select(x => new AppointmentArchiveViewModel()
                {
                    Date = x.Date.ToLocalTime().ToString("MM-dd-yyyy"),
                    ServiceName = x.Service.Name,
                    Status = x.IsDone ? "Finished" : "Canceled"
                })
                .ToList();

        public ICollection<AppointmentViewModel> GetUserAppointments(string userId, string specialistId = null)
            => this.data
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

        private Appointment GetAppointment(string appointmentId)
            => this.data
                .Appointments
                .FirstOrDefault(x => x.Id == appointmentId);
    }
}

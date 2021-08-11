using System;
using System.Linq;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;

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
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Models.Appointments;

namespace MediService.ASP.NET_Core.Services.Appointments
{
    public interface IAppointmentService
    {
        int GetUserAppointmetsCount(string userId);

        bool CanMakeAppointmentFromDate(DateTime date);

        Task<int> ArchiveAppointments();

        AppointmentDetailsViewModel GetAppointmentDetails(string appointmentId);

        ICollection<AppointmentArchiveViewModel> GetArchivedAppointments(string userId, string specialistId = null);

        ICollection<AppointmentViewModel> GetUserAppointments(string userId, string specialistId = null);

        Task<bool> FinishAppointment(string appointmentId);

        Task<bool> CancelAppointment(string appointmentId, string userId);

        Task<string> CreateAppointment
            (string additionalInfo,
             int serviceId,
             string specialistId,
             DateTime date,
             string userId,
             bool isCanceled = false,
             bool isDone = false);
    }
}

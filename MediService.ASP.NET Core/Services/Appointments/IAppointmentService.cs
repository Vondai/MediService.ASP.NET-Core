using System;
using System.Threading.Tasks;

namespace MediService.ASP.NET_Core.Services.Appointments
{
    public interface IAppointmentService
    {
        int GetUserAppointmetsCount(string userId);

        bool CanMakeAppointmentFromDate(DateTime date);

        Task<int> ArchiveAppointments(string userId, string specialistId = null);

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

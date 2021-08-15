using System.Collections.Generic;
using MediService.ASP.NET_Core.Models.Appointments;
using MediService.ASP.NET_Core.Models.Services;

namespace MediService.Test.Data
{
    public static class Appointments
    {
        private const string testAddress = "TestAddress";
        public static AppointmentFormModel AppointmentFormModelTest()
        {
            var model = new AppointmentFormModel()
            {
                Address = testAddress,
                Services = new List<ServiceViewFormModel>(),
                AppointmentsLeft = 10
            };

            return model;
        }

        public static AppointmentFormModel InvalidModel()
        {
            var model = new AppointmentFormModel()
            {
                Address = "",
                Date = "",
            };

            return model;
        }
    }
}

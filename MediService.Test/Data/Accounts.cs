using MediService.ASP.NET_Core.Data.Models;
using System;
using System.Collections.Generic;

namespace MediService.Test.Data
{
    public static class Accounts
    {
        private const string userId = "TestId";

        public static User ValidUser()
        {
            var user = new User() { Id = userId };

            return user;
        }
        public static Specialist UserSpecilist()
        {
            var specialist = new Specialist()
            {
                UserId = userId
            };

            return specialist;
        }

        public static User UserWithActiveAppointment()
        {
            var user = new User() { Id = userId };
            user.Appointments.Add(new Appointment() 
            {
                IsCanceled = false,
                IsDone = false,
            });

            return user;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using MediService.ASP.NET_Core.Data.Models;

namespace MediService.Test.Data
{
    public class Specialists
    {
        public static ICollection<Specialist> TenSpecialists()
        {
            var specialists = new List<Specialist>();
            for (int i = 0; i < 10; i++)
            {
                specialists.Add(new Specialist() { UserId = $"TestId{i}" });
            }
            return specialists;
        }
    }
}

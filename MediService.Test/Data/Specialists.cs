using System.Collections.Generic;
using System.Linq;
using MediService.ASP.NET_Core.Data.Models;

namespace MediService.Test.Data
{
    public class Specialists
    {
        public static IEnumerable<Specialist> TenSpecialists
            => Enumerable.Range(0, 10).Select(s => new Specialist());
    }
}

using System.Collections.Generic;
using System.Linq;
using MediService.ASP.NET_Core.Data.Models;

namespace MediService.Test.Data
{
    public class Services
    {
        public static IEnumerable<Service> TenServices
            => Enumerable.Range(0, 10).Select(r => new Service());
    }
}

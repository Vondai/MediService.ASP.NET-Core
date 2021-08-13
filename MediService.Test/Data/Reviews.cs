using System.Collections.Generic;
using System.Linq;
using MediService.ASP.NET_Core.Data.Models;

namespace MediService.Test.Data
{
    public static class Reviews
    {
        public static IEnumerable<Review> TenReviews
             => Enumerable.Range(0, 10).Select(r => new Review());
    }
}

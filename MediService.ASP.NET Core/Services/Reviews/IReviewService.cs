using System.Collections.Generic;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Models.Reviews;

namespace MediService.ASP.NET_Core.Services.Reviews
{
    public interface IReviewService
    {
        ICollection<ReviewViewModel> GetRecent();

        Task<int> Create(string title, string description, int rating, string userId);

        bool IsValidRating(int rating);
    }
}

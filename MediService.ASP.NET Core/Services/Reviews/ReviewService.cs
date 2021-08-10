using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Enums;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Reviews;

namespace MediService.ASP.NET_Core.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly MediServiceDbContext data;

        public ReviewService(MediServiceDbContext data)
        {
            this.data = data;
        }

        public async Task<int> Create(string title, string description, int rating, string userId)
        {
            var review = new Review()
            {
                Title = title,
                Description = description,
                Rating = rating,
                UserId = userId,
            };
            await this.data.Reviews.AddAsync(review);
            await this.data.SaveChangesAsync();

            return review.Id;
        }

        public ICollection<ReviewViewModel> GetRecent()
            => this.data.Reviews
                   .OrderByDescending(x => x.Rating)
                   .Select(x => new ReviewViewModel()
                   {
                       Title = x.Title,
                       Description = x.Description,
                       Rating = ((Rating)x.Rating).ToString(),
                       Username = this.data.Users
                       .Where(u => u.Id == x.UserId)
                       .Select(u => u.UserName)
                       .FirstOrDefault(),
                   })
                   .Take(4)
                   .ToList();

        public bool IsValidRating(int rating)
            => Enum.IsDefined(typeof(Rating), rating);
    }
}

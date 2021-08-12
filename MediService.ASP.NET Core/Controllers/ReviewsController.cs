using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Models.Reviews;
using MediService.ASP.NET_Core.Infrastructure;
using MediService.ASP.NET_Core.Services.Reviews;

namespace MediService.ASP.NET_Core.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewService reviews;

        public ReviewsController(IReviewService reviews)
        {
            this.reviews = reviews;
        }

        [Authorize]
        public IActionResult Create()
        {
            var userId = this.User.Id();
            var hasReview = this.reviews.HasReview(userId);
            if (hasReview)
            {
                TempData.Add("Error", "Users may make one review only.");
                return Redirect("/Home");
            }
            return View(new CreateReviewFormModel());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateReviewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isValidRating = this.reviews.IsValidRating(model.Rating);
            if (!isValidRating)
            {
                ModelState.AddModelError(nameof(model.Rating), "Invalid rating.");
                return View(model);
            }

            var userId = this.User.Id();
            await this.reviews.Create(model.Title, model.Description, model.Rating, userId);

            TempData.Add("Success", "Thank you for the review!");
            return Redirect("/Home");
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Models.Reviews;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Data.Enums;
using MediService.ASP.NET_Core.Infrastructure;

namespace MediService.ASP.NET_Core.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly MediServiceDbContext data;

        public ReviewsController(MediServiceDbContext data)
        {
            this.data = data;
        }

        public IActionResult Create() 
        {
            var userId = this.User.Id();
            if (userId == null)
            {
                TempData.Add("Error", "Please register first.");
                return Redirect("/Account/Register");
            }
            return View(new CreateReviewFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReviewFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isValidRating = Enum.IsDefined(typeof(Rating), model.Rating);
            if (!isValidRating)
            {
                ModelState.AddModelError(nameof(model.Rating), $"Invalid rating.");
                return View(model);
            }
            var userId = this.data.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(x => x.Id)
                .FirstOrDefault();
            var review = new Review()
            {
                Title = model.Title,
                Description = model.Description,
                Rating = model.Rating,
                UserId = userId,
            };
            await this.data.Reviews.AddAsync(review);
            await this.data.SaveChangesAsync();

            return Redirect("/Home");
        }
    }
}

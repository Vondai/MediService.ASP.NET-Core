using MediService.ASP.NET_Core.Data.Enums;
using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Models.Reviews
{
    public class CreateReviewFormModel
    {
        [Required]
        [StringLength(
            ReviewTitleMaxLength,
            MinimumLength = ReviewTitleMinLength,
            ErrorMessage = "Title must be between {2} and {1} characters long.")]
        public string Title { get; init; }

        [Required]
        [StringLength(
            ReviewDescriptionMaxLength,
            MinimumLength = ReviewDescriptionMinLength,
            ErrorMessage = "Description must be between {2} and {1} characters long.")]
        public string Description { get; init; }

        public int Rating { get; init; }
    }
}

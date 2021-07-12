using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ReviewTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(ReviewDescriptionMaxLength)]
        public string Description { get; set; }

        public int Rating { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}

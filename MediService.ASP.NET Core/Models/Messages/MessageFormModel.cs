using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Models.Messages
{
    public class MessageFormModel
    {
        [Required]
        [StringLength(
            MessageTitleMaxLength,
            ErrorMessage = "Title must be between {2} and {1} characters long.",
            MinimumLength = MessageTitleMinLength)]
        public string Title { get; init; }

        [Required]
        [StringLength(
            MessageContentMaxLength,
            ErrorMessage = "Content must be between {2} and {1} characters long.",
            MinimumLength = MessageContentMinLength)]
        public string Content { get; init; }
    }
}

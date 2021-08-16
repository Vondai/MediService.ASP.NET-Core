using System.ComponentModel.DataAnnotations;
using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Models.Services
{
    public class ServiceFormModel
    {
        [Required]
        [StringLength(ServiceNameMaxLength, 
            MinimumLength = ServiceNameMinLength,
            ErrorMessage = "Name must be atleast {2} characters long.")]
        public string Name { get; init; }

        [Required]
        [StringLength(
            int.MaxValue,
            MinimumLength = ServiceDescriptionMinLength,
            ErrorMessage = "Description must be atleast {2} characters long.")]
        public string Description { get; init; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediService.ASP.NET_Core.Models.Services;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Models.Specialists
{
    public class SpecialistAddFormModel
    {

        [Required(ErrorMessage = "Enter an username")]
        public string Username { get; init; }

        [Required(ErrorMessage = "Enter a description")]
        [StringLength(SpecialistDescriptionMaxLength,
            MinimumLength = SpecialistDescriptionMinLength,
            ErrorMessage = "Description must be between {2} and {1} characters long.")]
        public string Description { get; init; }

        [Display(Name = "Medical service")]
        public int ServiceId { get; init; }

        public IEnumerable<ServiceViewFormModel> Services { get; set; }
    }
}

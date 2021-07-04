using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;
namespace MediService.ASP.NET_Core.Models.Users
{
    public class UserLoginFormModel
    {
        [Required(ErrorMessage = UsernameNullErrorMessage)]
        public string Username { get; init; }

        [Required(ErrorMessage = PasswordNullErrorMessage)]
        public string Password { get; init; }
    }
}

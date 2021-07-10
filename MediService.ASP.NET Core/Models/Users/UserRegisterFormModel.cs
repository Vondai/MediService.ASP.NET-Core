using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Models.Users
{
    public class UserRegisterFormModel
    {
        [Required(ErrorMessage = UsernameNullErrorMessage)]
        [MinLength(UsernameMinLength, ErrorMessage = UsernameMinLengthErrorMessage)]
        [MaxLength(UsernameMaxLength, ErrorMessage = UsernameMaxLengthErrorMessage)]
        public string Username { get; init; }

        [Required(ErrorMessage = PasswordNullErrorMessage)]
        [MinLength(PasswordMinLength, ErrorMessage = PasswordMinLengthErrorMessage)]
        [MaxLength(PasswordMaxLength, ErrorMessage = PasswordMaxLengthErrorMessage)]
        public string Password { get; init; }

        [Required(ErrorMessage = ConfirmPasswordNullErrorMessage)]
        [Compare(nameof(Password), ErrorMessage = PasswordsDifferentErrorMessage)]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; init; }

        [Required(ErrorMessage = EmailNullErrorMessage)]
        [EmailAddress(ErrorMessage = EmailInvalidFormatErrorMessage)]
        public string Email { get; init; }

        [Required(ErrorMessage = FullNameNullErrorMessage)]
        [Display(Name = "Full Name")]
        public string FullName { get; init; }

        [Required(ErrorMessage = PhoneNullErrorMessage)]
        [RegularExpression(PhoneRegularExpression, ErrorMessage = PhoneWrongFormatErrorMessage)]
        [Display(Name = "Phone Number")]
        public string Phone { get; init; }

        [Required(ErrorMessage = AddressNullErrorMessage)]
        public string Address { get; init; }

        [Required]
        public string City { get; init; }

    }
}

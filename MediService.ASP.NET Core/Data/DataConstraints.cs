namespace MediService.ASP.NET_Core.Data
{
    public static class DataConstraints
    {
        //User
        //Username
        public const int UsernameMinLength = 5;
        public const int UsernameMaxLength = 15;
        public const string UsernameNullErrorMessage = "Please enter an username.";
        public const string UsernameMinLengthErrorMessage = "Username must be atleast 5 characters long.";
        public const string UsernameMaxLengthErrorMessage = "Username cannot be more than 15 characters long.";

        //Password
        public const string PasswordNullErrorMessage = "Please enter a password.";
        public const string ConfirmPasswordNullErrorMessage = "Please enter a confirm password";
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 20;
        public const string PasswordMinLengthErrorMessage = "Password must be atleast 6 characters long.";
        public const string PasswordMaxLengthErrorMessage = "Password cannot be more than 20 characters long.";
        public const string PasswordsDifferentErrorMessage = "Password and confirm password must be the same";

        //Email
        public const string EmailNullErrorMessage = "Please enter an email address.";
        public const string EmailInvalidFormatErrorMessage = "Invalid email address format";

        //Full Name
        public const string FullNameNullErrorMessage = "Please enter your full name.";

        //Phone
        public const string PhoneNullErrorMessage = "Please enter your phone number";
        public const string PhoneRegularExpression = @"[0-9]{10}";
        public const string PhoneWrongFormatErrorMessage = "Phone number must consist of 10 digits";

        //Address
        public const string AddressNullErrorMessage = "Please enter an address.";

        //City
        public const string CityNullErrorMessage = "Please enter a city";

        //Subscriptions
        public const string SubscriptionMinValue = "0.0";

        public const string SubscriptionMaxValue = "100.0";
    }
}

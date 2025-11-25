using System.Text.RegularExpressions;

namespace JoyModels.Services.Validation;

public static class RegularExpressionValidation
{
    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty string");

        const string pattern = @"^(?=.{1,255})[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(email, pattern))
            throw new ArgumentException("Invalid format. The value must be a valid email address without spaces");
    }

    public static void ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty string");

        const string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";

        if (!Regex.IsMatch(password, pattern))
            throw new ArgumentException(
                "Password must be minimum 8 characters long and include at least one uppercase letter, one number, and one special character (@$!%*#?&).");
    }

    public static void ValidateNickname(string nickname)
    {
        if (string.IsNullOrWhiteSpace(nickname))
            throw new ArgumentException("Nickname cannot be empty string");

        const string pattern = @"^[\p{Ll}\p{Nd}]{3,}$";

        if (!Regex.IsMatch(nickname, pattern))
            throw new ArgumentException(
                "The value must be minimum 3 characters long and contain only lowercase letters and numbers.");
    }

    public static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty string");

        const string pattern = @"^\p{Lu}\p{Ll}{1,}$";

        if (!Regex.IsMatch(name, pattern))
            throw new ArgumentException(
                "First name must begin with a capital letter and contain only lowercase letters after.");
    }

    public static void ValidateOtpCode(string otpCode)
    {
        if (string.IsNullOrWhiteSpace(otpCode))
            throw new ArgumentException("OTP Code cannot be empty string");

        const string pattern = "^[A-Z0-9]{12}$";

        if (!Regex.IsMatch(otpCode, pattern))
            throw new ArgumentException(
                "The value must be exactly 12 characters long and contain only uppercase letters and numbers.");
    }

    public static void ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Input cannot be empty string");

        const string pattern = @"^[\p{L}\p{Nd}:.,\-\' ]{1,}$";

        if (!Regex.IsMatch(text, pattern))
            throw new ArgumentException(
                "Invalid value: Must contain only letters (any language), digits, and the following characters: (:.,-').");
    }
}
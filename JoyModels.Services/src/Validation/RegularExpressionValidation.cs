using System.Text.RegularExpressions;

namespace JoyModels.Services.Validation;

public static class RegularExpressionValidation
{
    public static bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty string");

        const string pattern = @"^(?=.{1,255})[^@\s]+@[^@\s]+\.[^@\s]+$";

        return Regex.IsMatch(email, pattern);
    }

    public static bool IsPasswordValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty string");

        const string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,255}$";

        return Regex.IsMatch(password, pattern);
    }

    public static bool IsNicknameValid(string nickName)
    {
        if (string.IsNullOrWhiteSpace(nickName))
            throw new ArgumentException("Nickname cannot be empty string");

        const string pattern = @"^[\p{Ll}\p{Nd}]{3,255}$";

        return Regex.IsMatch(nickName, pattern);
    }

    public static bool IsNameValid(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Name cannot be empty string");

        const string pattern = @"^\p{Lu}\p{Ll}{1,255}$";

        return Regex.IsMatch(input, pattern);
    }

    public static bool IsOtpCodeValid(string otpCode)
    {
        if (string.IsNullOrWhiteSpace(otpCode))
            throw new ArgumentException("OTP Code cannot be empty string");

        const string pattern = "^[A-Z0-9]{12}$";

        return Regex.IsMatch(otpCode, pattern);
    }

    public static bool IsStringValid(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty string");

        const string pattern = @"^[\p{L}\p{Nd}:.,\- ]{1,}$";

        return Regex.IsMatch(input, pattern);
    }
}
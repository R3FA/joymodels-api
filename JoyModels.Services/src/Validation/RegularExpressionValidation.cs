using System.Text.RegularExpressions;

namespace JoyModels.Services.Validation;

public static class RegularExpressionValidation
{
    public static bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty string");

        var pattern = @"^(?=.{1,255})[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, pattern))
            return false;

        return true;
    }

    public static bool IsPasswordValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty string");

        var pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,255}$";
        if (!Regex.IsMatch(password, pattern))
            return false;

        return true;
    }

    public static bool IsNicknameValid(string nickName)
    {
        if (string.IsNullOrWhiteSpace(nickName))
            throw new ArgumentException("Nickname cannot be empty string");

        var pattern = @"^[\p{Ll}\p{Nd}]{3,255}$";
        if (!Regex.IsMatch(nickName, pattern))
            return false;

        return true;
    }

    public static bool IsStringValid(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty string");

        var pattern = @"^\p{Lu}\p{Ll}{1,255}$";
        if (!Regex.IsMatch(input, pattern))
            return false;

        return true;
    }

    public static bool IsOtpCodeValid(string otpCode)
    {
        if (string.IsNullOrWhiteSpace(otpCode))
            throw new ArgumentException("OTP Code cannot be empty string");

        const string pattern = @"^[A-Z0-9]{12}$";
        if (!Regex.IsMatch(otpCode, pattern))
            return false;

        return true;
    }
}
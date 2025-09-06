using System.Text.RegularExpressions;

namespace JoyModels.Services.Validation;

public static class RegularExpressionValidation
{
    public static bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ApplicationException("Email cannot be empty string");

        const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, pattern))
            return false;

        return true;
    }

    public static bool IsPasswordValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ApplicationException("Password cannot be empty string");

        const string pattern = @"^(?=.{8,})(?=.\p{Lu})(?=.\p{Nd})(?=.[!@#$%^&])[\p{L}\p{Nd}!@#$%^&*]+";
        if (!Regex.IsMatch(password, pattern))
            return false;

        return true;
    }

    public static bool IsStringValid(string input, int inputLength)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ApplicationException("Input cannot be empty string");

        var pattern = $@"^\p{{Lu}}\p{{Ll}}{{1,{inputLength - 1}}}$";
        if (!Regex.IsMatch(input, pattern))
            return false;

        return true;
    }
}
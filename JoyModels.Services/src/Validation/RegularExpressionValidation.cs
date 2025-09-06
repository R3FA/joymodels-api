using System.Text.RegularExpressions;

namespace JoyModels.Services.Validation;

public static class RegularExpressionValidation
{
    public static bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(email, pattern))
            return false;

        return true;
    }

    public static bool IsStringValid(string input, int inputLength)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var pattern = $@"^\p{{Lu}}\p{{Ll}}{{1,{inputLength - 1}}}$";

        if (!Regex.IsMatch(input, pattern))
            return false;

        return true;
    }
}
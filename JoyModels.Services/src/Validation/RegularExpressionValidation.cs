using System.Text.RegularExpressions;

namespace JoyModels.Services.Validation;

public static class RegularExpressionValidation
{
    public static bool IsEmailValid(string email, int emailMaxLength)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ApplicationException("Email cannot be empty string");

        var pattern = $@"^(?=.{{1,{emailMaxLength - 1}}})[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(email, pattern))
            return false;

        return true;
    }

    public static bool IsPasswordValid(string password, int passwordMaxLength)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ApplicationException("Password cannot be empty string");

        var pattern = $@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{{8,{passwordMaxLength - 1}}}$";
        if (!Regex.IsMatch(password, pattern))
            return false;

        return true;
    }

    public static bool IsNicknameValid(string nickName, int nickNameMaxLength)
    {
        if (string.IsNullOrWhiteSpace(nickName))
            throw new ApplicationException("Nickname cannot be empty string");

        var pattern = $@"^[\p{{Ll}}\p{{Nd}}]{{3,{nickNameMaxLength - 1}}}$";
        if (!Regex.IsMatch(nickName, pattern))
            return false;

        return true;
    }

    public static bool IsStringValid(string input, int inputMaxLength)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ApplicationException("Input cannot be empty string");

        var pattern = $@"^\p{{Lu}}\p{{Ll}}{{1,{inputMaxLength - 1}}}$";
        if (!Regex.IsMatch(input, pattern))
            return false;

        return true;
    }
}
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.Sso;

public static class SsoHelperMethods
{
    public static void ValidateUserCreation(this UserCreate user)
    {
        if (!RegularExpressionValidation.IsStringValid(user.FirstName,
                Validation.ConstantValidation.User.UserCreate.FirstNameMaxLength))
            throw new ArgumentException($"First name `{user.FirstName}` is invalid");

        if (user.LastName != null)
        {
            if (!RegularExpressionValidation.IsStringValid(user.LastName,
                    Validation.ConstantValidation.User.UserCreate.LastNameMaxLength))
                throw new ArgumentException($"Last name `{user.LastName}` is invalid");
        }

        if (!RegularExpressionValidation.IsNicknameValid(user.Nickname,
                Validation.ConstantValidation.User.UserCreate.NicknameMaxLength))
            throw new ArgumentException($"Nickname `{user.Nickname}` is invalid");

        if (!RegularExpressionValidation.IsEmailValid(user.Email,
                Validation.ConstantValidation.User.UserCreate.EmailMaxLength))
            throw new ArgumentException($"Email `{user.Email}` is invalid");

        if (!RegularExpressionValidation.IsPasswordValid(user.Password,
                Validation.ConstantValidation.User.UserCreate.PasswordMaxLength))
            throw new ArgumentException($"Password `{user.Password}` is invalid");
    }
}
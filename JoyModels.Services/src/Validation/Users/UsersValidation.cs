using JoyModels.Models.DataTransferObjects.RequestTypes.Users;

namespace JoyModels.Services.Validation.Users;

public static class UsersValidation
{
    private static void ValidateNickname(string nickname)
    {
        if (!RegularExpressionValidation.IsNicknameValid(nickname))
            throw new ArgumentException(
                "Nickname must have at least 3 characters and may only contain lowercase letters and numbers.");
    }

    public static void ValidateUserSearchArguments(this UsersSearchRequest request)
    {
        if (request.Nickname != null)
            ValidateNickname(request.Nickname);
    }
}
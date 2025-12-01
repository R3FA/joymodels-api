using JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;

namespace JoyModels.Services.Validation;

public static class UserRoleValidation
{
    private static void ValidateRoleName(string roleName)
    {
        if (!string.IsNullOrWhiteSpace(roleName))
            RegularExpressionValidation.ValidateName(roleName);
    }

    public static void ValidateUserRoleSearchArguments(this UserRoleSearchRequest request)
        => ValidateRoleName(request.RoleName!);
}
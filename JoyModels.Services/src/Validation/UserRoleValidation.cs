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
    {
        if (!string.IsNullOrWhiteSpace(request.RoleName))
            RegularExpressionValidation.ValidateText(request.RoleName);
    }

    public static void ValidateUserRoleCreateArguments(this UserRoleCreateRequest request)
        => ValidateRoleName(request.RoleName);

    public static void ValidateUserRolePatchArguments(this UserRolePatchRequest request)
        => ValidateRoleName(request.RoleName);
}
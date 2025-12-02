using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;

namespace JoyModels.Services.Validation;

public static class CommunityPostTypeValidation
{
    private static void ValidatePostTypeName(string postTypeName)
    {
        if (!string.IsNullOrWhiteSpace(postTypeName))
            RegularExpressionValidation.ValidateName(postTypeName);
    }

    public static void ValidateCommunityPostTypeSearchArguments(this CommunityPostTypeSearchRequest request)
        => ValidatePostTypeName(request.PostTypeName!);

    public static void ValidateCommunityPostCreateArguments(this CommunityPostTypeCreateRequest request)
        => ValidatePostTypeName(request.PostTypeName);

    public static void ValidateCommunityPostPatchArguments(this CommunityPostTypePatchRequest request)
        => ValidatePostTypeName(request.PostTypeName);
}
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;

namespace JoyModels.Services.Validation;

public static class CommunityPostReviewTypeValidation
{
    private static void ValidatePostTypeName(string postTypeName)
    {
        if (!string.IsNullOrWhiteSpace(postTypeName))
            RegularExpressionValidation.ValidateName(postTypeName);
    }

    public static void ValidateCommunityPostReviewTypeSearchArguments(this CommunityPostReviewTypeSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.CommunityPostReviewTypeName))
            RegularExpressionValidation.ValidateText(request.CommunityPostReviewTypeName);
    }

    public static void ValidateCommunityPostReviewTypeCreateArguments(this CommunityPostReviewTypeCreateRequest request)
        => ValidatePostTypeName(request.CommunityPostReviewTypeName);

    public static void ValidateCommunityPostReviewTypePatchArguments(this CommunityPostReviewTypePatchRequest request)
        => ValidatePostTypeName(request.CommunityPostReviewTypeName);
}
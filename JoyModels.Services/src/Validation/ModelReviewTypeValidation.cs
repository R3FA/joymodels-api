using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;

namespace JoyModels.Services.Validation;

public static class ModelReviewTypeValidation
{
    private static void ValidateModelReviewTypeName(string modelReviewTypeName)
    {
        if (!string.IsNullOrWhiteSpace(modelReviewTypeName))
            RegularExpressionValidation.ValidateName(modelReviewTypeName);
    }

    public static void ValidateModelReviewTypeSearchArguments(this ModelReviewTypeSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelReviewTypeName))
            RegularExpressionValidation.ValidateText(request.ModelReviewTypeName);
    }

    public static void ValidateModelReviewTypeCreateArguments(this ModelReviewTypeCreateRequest request)
        => ValidateModelReviewTypeName(request.ModelReviewTypeName);

    public static void ValidateModelReviewTypePatchArguments(this ModelReviewTypePatchRequest request)
        => ValidateModelReviewTypeName(request.ModelReviewTypeName);
}
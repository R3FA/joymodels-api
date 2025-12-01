using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;

namespace JoyModels.Services.Validation;

public static class ModelReviewTypeValidation
{
    public static void ValidateModelReviewTypeSearchArguments(this ModelReviewTypeSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelReviewTypeName))
            RegularExpressionValidation.ValidateName(request.ModelReviewTypeName);
    }
}
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation.ModelReviews;

public static class ModelReviewValidation
{
    private static void ValidateModelReviewStringArguments(string stringArgument)
    {
        if (!RegularExpressionValidation.IsStringValid(stringArgument))
            throw new ArgumentException(
                "Invalid value: Must contain only letters (any language), digits, and the following characters: ':', '.', ',', '-'.");
    }

    public static void ValidateModelReviewCreateArguments(this ModelReviewCreateRequest request,
        ModelResponse modelResponse, Guid userClaimUuid)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelReviewText))
            ValidateModelReviewStringArguments(request.ModelReviewText);

        if (modelResponse.UserUuid == userClaimUuid)
            throw new ApplicationException("You cannot review your own model!");
    }

    public static void ValidateModelReviewPatchArguments(this ModelReviewPatchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelReviewText))
            ValidateModelReviewStringArguments(request.ModelReviewText);
    }

    public static async Task ValidateDuplicatedModelReviews(JoyModelsDbContext context, Guid modelUuid,
        Guid userClaimUuid)
    {
        var isDuplicated = await context.ModelReviews.AnyAsync(x => x.ModelUuid == modelUuid
                                                                    && x.UserUuid == userClaimUuid);
        if (isDuplicated)
            throw new ApplicationException("You have already reviewed this model!");
    }
}
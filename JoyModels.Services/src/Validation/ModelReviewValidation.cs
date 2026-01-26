using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation;

public static class ModelReviewValidation
{
    public static void ValidateModelReviewCreateArguments(this ModelReviewCreateRequest request,
        ModelResponse modelResponse, Guid userClaimUuid)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelReviewText))
            RegularExpressionValidation.ValidateText(request.ModelReviewText);

        if (modelResponse.UserUuid == userClaimUuid)
            throw new ApplicationException("You cannot review your own model!");
    }

    public static void ValidateModelReviewPatchArguments(this ModelReviewPatchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelReviewText))
            RegularExpressionValidation.ValidateText(request.ModelReviewText);
    }

    public static async Task ValidateDuplicatedModelReviews(JoyModelsDbContext context, Guid modelUuid,
        Guid userClaimUuid)
    {
        var isDuplicated = await context.ModelReviews.AnyAsync(x => x.ModelUuid == modelUuid
                                                                    && x.UserUuid == userClaimUuid);
        if (isDuplicated)
            throw new ApplicationException("You have already reviewed this model!");
    }

    public static async Task ValidateModelOwnership(JoyModelsDbContext context, Guid modelUuid, Guid userClaimUuid)
    {
        var hasInLibrary = await context.Libraries
            .AnyAsync(x => x.UserUuid == userClaimUuid && x.ModelUuid == modelUuid);

        if (!hasInLibrary)
            throw new ApplicationException("You can only review models you have purchased!");
    }
}
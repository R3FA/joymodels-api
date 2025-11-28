using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation;

public static class CommunityPostValidation
{
    public static async Task ValidateCommunityPostCreateArguments(this CommunityPostCreateRequest request,
        ModelImageSettingsDetails modelImageSettingsDetails)
    {
        RegularExpressionValidation.ValidateText(request.Title);
        RegularExpressionValidation.ValidateText(request.Description);

        if (!string.IsNullOrWhiteSpace(request.YoutubeVideoLink))
            RegularExpressionValidation.ValidateYoutubeVideoLink(request.YoutubeVideoLink);

        if (request.Pictures != null)
        {
            foreach (var picture in request.Pictures)
            {
                await GlobalValidation.ValidateModelAndCommunityPostPicture(picture, modelImageSettingsDetails);
            }
        }
    }

    public static async Task ValidateCommunityPostLikeArguments(this CommunityPostUserReviewCreateRequest request,
        JoyModelsDbContext context, UserAuthValidation userAuthValidation)
    {
        var isDataDuplicated = await context.CommunityPostUserReviews
            .Where(x => x.CommunityPostUuid == request.CommunityPostUuid
                        && x.UserUuid == userAuthValidation.GetUserClaimUuid())
            .AnyAsync(x => x.ReviewTypeUuid == request.ReviewTypeUuid);

        if (isDataDuplicated)
            throw new ArgumentException("You cannot review the same community post twice.");
    }
}
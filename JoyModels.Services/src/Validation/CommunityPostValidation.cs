using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Validation;

public static class CommunityPostValidation
{
    public static void ValidateCommunityPostSearchArguments(this CommunityPostSearchRequest request,
        UserAuthValidation userAuthValidation)
    {
        if (!string.IsNullOrWhiteSpace(request.Title))
            RegularExpressionValidation.ValidateText(request.Title);

        if (request.UserUuid.HasValue)
            userAuthValidation.ValidateUserAuthRequest(request.UserUuid.Value);
    }

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

    public static async Task ValidateCommunityPostPatchArguments(
        this CommunityPostPatchRequest request,
        JoyModelsDbContext context,
        CommunityPostResponse communityPostResponse,
        ModelImageSettingsDetails modelImageSettingsDetails)
    {
        if (string.IsNullOrWhiteSpace(request.Title)
            && string.IsNullOrWhiteSpace(request.Description)
            && request.PostTypeUuid == null
            && string.IsNullOrWhiteSpace(request.YoutubeVideoLink)
            && (request.PicturesToAdd == null || request.PicturesToAdd.Count == 0)
            && (request.PicturesToRemove == null || request.PicturesToRemove.Count == 0))
            throw new ArgumentException("You cannot send an empty request!");

        if (!string.IsNullOrWhiteSpace(request.Title))
            RegularExpressionValidation.ValidateText(request.Title);

        if (!string.IsNullOrWhiteSpace(request.Description))
            RegularExpressionValidation.ValidateText(request.Description);

        if (!string.IsNullOrWhiteSpace(request.YoutubeVideoLink))
            RegularExpressionValidation.ValidateYoutubeVideoLink(request.YoutubeVideoLink);

        if (request.PicturesToRemove != null)
        {
            foreach (var potentialRemovablePictureLocation in request.PicturesToRemove)
            {
                var exists = await context.CommunityPostPictures.AnyAsync(x =>
                    string.Equals(x.PictureLocation, potentialRemovablePictureLocation));

                if (!exists)
                    throw new KeyNotFoundException(
                        $"Location {potentialRemovablePictureLocation} for this picture doesn't exit!");
            }
        }

        if (request.PicturesToAdd != null)
        {
            if (communityPostResponse.PictureLocations.Count > 0)
            {
                var totalNumberOfPictures = communityPostResponse.PictureLocations.Count + request.PicturesToAdd.Count;

                if (totalNumberOfPictures > 4)
                    throw new ArgumentException(
                        $"Cannot add {request.PicturesToAdd.Count} picture(s).  Community post already has {communityPostResponse.PictureLocations.Count} picture(s) and the maximum allowed is 4.");
            }

            foreach (var picture in request.PicturesToAdd)
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
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;

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
}
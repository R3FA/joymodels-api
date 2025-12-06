using JoyModels.Models.DataTransferObjects.ImageSettings;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace JoyModels.Services.Validation;

public static class GlobalValidation
{
    public static async Task ValidateModelAndCommunityPostPicture(IFormFile modelPicture,
        ModelImageSettingsDetails modelImageSettingsDetails)
    {
        if (modelPicture.Length > modelImageSettingsDetails.AllowedSize)
            throw new ArgumentException("Image too large. Maximum size limit is 10MB");

        await using (var s1 = modelPicture.OpenReadStream())
        {
            var format = await Image.DetectFormatAsync(s1);
            if (format is null || (format != JpegFormat.Instance && format != PngFormat.Instance))
                throw new ArgumentException("Unsupported image format. Allowed: .jpg, .jpeg, .png");
        }

        await using var s2 = modelPicture.OpenReadStream();
        var info = await Image.IdentifyAsync(s2);
        if (info == null)
            throw new ArgumentException("Unsupported or corrupted image.");

        var minWidth = modelImageSettingsDetails.ImageSettingsResolutionDetails.MinimumWidth;
        var maxWidth = modelImageSettingsDetails.ImageSettingsResolutionDetails.MaximumWidth;
        var minHeight = modelImageSettingsDetails.ImageSettingsResolutionDetails.MinimumHeight;
        var maxHeight = modelImageSettingsDetails.ImageSettingsResolutionDetails.MaximumHeight;

        if (info.Width < minWidth || info.Width > maxWidth || info.Height < minHeight || info.Height > maxHeight)
            throw new ArgumentException(
                $"Image error: {info.Width}x{info.Height}. Allowed: width between {minWidth}-{maxWidth}px and height between {minHeight}-{maxHeight}px.");
    }
}
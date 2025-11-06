using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace JoyModels.Services.Validation.Models;

public static class ModelValidation
{
    private static void ValidateModelStringArguments(string stringArgument)
    {
        if (!RegularExpressionValidation.IsStringValid(stringArgument))
            throw new ArgumentException(
                "Invalid value: Must contain only letters (any language), digits, and the following characters: ':', '.', ',', '-'.");
    }

    public static void ValidateModelSearchArguments(this ModelSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelName))
            ValidateModelStringArguments(request.ModelName);
    }

    public static void ValidateModelCreateArguments(this ModelCreateRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Name))
            ValidateModelStringArguments(request.Name);

        if (!string.IsNullOrWhiteSpace(request.Description))
            ValidateModelStringArguments(request.Description);

        if (request.Price <= 0)
            throw new ArgumentException("Price must be a positive number.");

        if (request.ModelCategoryUuids.Count == 0)
            throw new ArgumentException("ModelCategoryUuids must be specified.");
    }

    public static async Task ValidateModelPicture(IFormFile modelPicture,
        ImageSettingsDetails imageSettingsDetails)
    {
        if (modelPicture.Length > imageSettingsDetails.AllowedSize)
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

        var minWidth = imageSettingsDetails.ImageSettingsResolutionDetails.MinimumWidth;
        var maxWidth = imageSettingsDetails.ImageSettingsResolutionDetails.MaximumWidth;
        var minHeight = imageSettingsDetails.ImageSettingsResolutionDetails.MinimumHeight;
        var maxHeight = imageSettingsDetails.ImageSettingsResolutionDetails.MaximumHeight;

        if (info.Width < minWidth || info.Width > maxWidth || info.Height < minHeight || info.Height > maxHeight)
            throw new ArgumentException(
                $"Image error: {info.Width}x{info.Height}. Allowed: width between {minWidth}-{maxWidth}px and height between {minHeight}-{maxHeight}px.");
    }

    public static void ValidateModel(IFormFile model,
        ModelSettingsDetails modelSettingsDetails)
    {
        if (model.Length > modelSettingsDetails.AllowedSize)
            throw new ArgumentException("Model too large. Maximum size limit is 30MB");

        var format = Path.GetExtension(model.FileName).ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentException("Model has no format!");

        if (!modelSettingsDetails.AllowedFormats.Any(x => string.Equals(x, format)))
            throw new ArgumentException(
                "Unsupported model format. Allowed: .glb,.gltf,.fbx,.obj,.stl,.blend,.max,.ma,.mb");
    }
}
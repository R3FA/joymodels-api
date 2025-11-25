using System.Data;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using ModelAvailabilityEnum = JoyModels.Models.Enums.ModelAvailability;

namespace JoyModels.Services.Validation;

public static class ModelValidation
{
    public static void ValidateModelSearchArguments(string requestName)
    {
        if (!string.IsNullOrWhiteSpace(requestName))
            RegularExpressionValidation.ValidateText(requestName);
    }

    public static void ValidateModelCreateArguments(this ModelCreateRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Name))
            RegularExpressionValidation.ValidateText(request.Name);

        if (!string.IsNullOrWhiteSpace(request.Description))
            RegularExpressionValidation.ValidateText(request.Description);

        if (request.Price <= 0)
            throw new ArgumentException("Price must be a positive number.");

        if (request.ModelCategoryUuids.Count == 0)
            throw new ArgumentException("ModelCategoryUuids must be specified.");
    }

    public static async Task ValidateModelPicture(IFormFile modelPicture,
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

    public static async Task ValidateModelLikeEndpoint(JoyModelsDbContext context, Guid modelUuid,
        UserAuthValidation userAuthValidation)
    {
        var isModelPublic = await context.Models
            .Include(x => x.ModelAvailabilityUu)
            .AnyAsync(x => x.Uuid == modelUuid
                           && x.ModelAvailabilityUu.AvailabilityName == nameof(ModelAvailabilityEnum.Public));

        if (!isModelPublic)
            throw new ArgumentException("You cannot like a hidden model.");

        var exists = await context.UserModelLikes
            .AnyAsync(x =>
                x.UserUuid == userAuthValidation.GetUserClaimUuid()
                && x.ModelUuid == modelUuid);
        if (exists)
            throw new ArgumentException("You have already liked this model.");
    }

    public static void ValidateModelPatchArguments(this ModelPatchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name)
            && string.IsNullOrWhiteSpace(request.Description)
            && request.Price == null
            && request.ModelAvailabilityUuid == null
            && (request.ModelCategoriesToDelete == null || request.ModelCategoriesToDelete.Count == 0)
            && (request.ModelCategoriesToInsert == null || request.ModelCategoriesToInsert.Count == 0)
            && (request.ModelPictureLocationsToDelete == null || request.ModelPictureLocationsToDelete.Count == 0)
            && (request.ModelPictureToInsert == null || request.ModelPictureToInsert.Count == 0))
            throw new ArgumentException("You cannot send an empty request!");

        if (!string.IsNullOrWhiteSpace(request.Name))
            RegularExpressionValidation.ValidateText(request.Name);

        if (!string.IsNullOrWhiteSpace(request.Description))
            RegularExpressionValidation.ValidateText(request.Description);

        if (request.Price is not null && request.Price <= 0)
            throw new ArgumentException("Price must be a positive number.");
    }

    public static async Task ValidateModelPatchArgumentsDuplicatedFields(this ModelPatchRequest request,
        JoyModelsDbContext context)
    {
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var isModelNameDuplicated = await context.Models.AnyAsync(x => string.Equals(x.Name, request.Name));
            if (isModelNameDuplicated)
                throw new DuplicateNameException(
                    $"Model name `{request.Name}` is already registered in our database.");
        }
    }

    public static async Task ValidateModelUnlikeEndpoint(JoyModelsDbContext context, Guid modelUuid,
        UserAuthValidation userAuthValidation)
    {
        var exists = await context.UserModelLikes
            .AnyAsync(x =>
                x.UserUuid == userAuthValidation.GetUserClaimUuid() && x.ModelUuid == modelUuid);
        if (!exists)
            throw new ArgumentException("You cannot unlike a model that you have never liked.");
    }
}
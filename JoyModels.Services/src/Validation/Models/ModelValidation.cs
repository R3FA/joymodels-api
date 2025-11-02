using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using Microsoft.AspNetCore.Http;

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

        if (request.ModelCategoryUuids.Length == 0)
            throw new ArgumentException("ModelCategoryUuids must be specified.");
    }

    public static void ValidateModelPictureExtension(IFormFile modelPicture, ImageSettingsDetails imageSettingsDetails)
    {
        var fileExtension = Path.GetExtension(modelPicture.FileName);

        var counter = 0;
        foreach (var allowedExtension in imageSettingsDetails.AllowedExtensions)
        {
            if (allowedExtension != fileExtension)
                counter++;
        }

        if (counter >= imageSettingsDetails.AllowedExtensions.Length)
            throw new ArgumentException(
                $"File extension {fileExtension} is not supported. It must be .jpg, .jpeg or .png");
    }
}
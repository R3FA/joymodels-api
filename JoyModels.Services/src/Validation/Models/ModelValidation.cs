using JoyModels.Models.DataTransferObjects.RequestTypes.Models;

namespace JoyModels.Services.Validation.Models;

public static class ModelValidation
{
    private static void ValidateModelName(string modelName)
    {
        if (!RegularExpressionValidation.IsStringValid(modelName))
            throw new ArgumentException(
                "Invalid value: must be 1–100 characters long and contain only letters (any language), digits, and the following characters: ':', '.', ',', '-'.");
    }

    public static void ValidateModelSearchArguments(this ModelSearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.ModelName))
            ValidateModelName(request.ModelName);
    }
}
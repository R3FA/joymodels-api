using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

namespace JoyModels.Services.Validation.Categories;

public static class CategoryValidation
{
    private static void ValidateModelName(string modelName)
    {
        if (!RegularExpressionValidation.IsStringValid(modelName))
            throw new ArgumentException(
                "Invalid value: must be 1–100 characters long and contain only letters (any language), digits, and the following characters: ':', '.', ',', '-'.");
    }

    public static void ValidateCategorySearchArguments(this CategorySearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.CategoryName))
            ValidateModelName(request.CategoryName);
    }
}
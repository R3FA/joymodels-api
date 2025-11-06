using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

namespace JoyModels.Services.Validation.Categories;

public static class CategoryValidation
{
    private static void ValidateString(string input)
    {
        if (!RegularExpressionValidation.IsStringValid(input))
            throw new ArgumentException(
                "Invalid value: Must contain only letters (any language), digits, and the following characters: ':', '.', ',', '-'.");
    }

    private static void ValidateCategoryName(string categoryName)
    {
        if (!string.IsNullOrWhiteSpace(categoryName))
            ValidateString(categoryName);
    }

    public static void ValidateCategorySearchArguments(this CategorySearchRequest request)
        => ValidateCategoryName(request.CategoryName!);

    public static void ValidateCategoryCreateArguments(this CategoryCreateRequest request)
        => ValidateCategoryName(request.CategoryName);

    public static void ValidateCategoryPatchArguments(this CategoryPatchRequest request)
        => ValidateCategoryName(request.CategoryName);
}
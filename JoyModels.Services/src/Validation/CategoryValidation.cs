using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;

namespace JoyModels.Services.Validation;

public static class CategoryValidation
{
    private static void ValidateCategoryName(string categoryName)
    {
        if (!string.IsNullOrWhiteSpace(categoryName))
            RegularExpressionValidation.ValidateText(categoryName);
    }

    public static void ValidateCategorySearchArguments(this CategorySearchRequest request)
        => ValidateCategoryName(request.CategoryName!);

    public static void ValidateCategoryCreateArguments(this CategoryCreateRequest request)
        => ValidateCategoryName(request.CategoryName);

    public static void ValidateCategoryPatchArguments(this CategoryPatchRequest request)
        => ValidateCategoryName(request.CategoryName);
}
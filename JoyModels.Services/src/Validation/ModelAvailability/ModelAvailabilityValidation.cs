using JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

namespace JoyModels.Services.Validation.ModelAvailability;

public static class ModelAvailabilityValidation
{
    private static void ValidateName(string input)
    {
        if (!RegularExpressionValidation.IsNameValid(input))
            throw new ArgumentException(
                "First name must begin with a capital letter and contain only lowercase letters after.");
    }

    private static void ValidateString(string input)
    {
        if (!RegularExpressionValidation.IsStringValid(input))
            throw new ArgumentException(
                "Invalid value: Must contain only letters (any language), digits, and the following characters: ':', '.', ',', '-'.");
    }

    private static void ValidateModelAvailabilityName(string modelAvailabilityName)
    {
        if (!string.IsNullOrWhiteSpace(modelAvailabilityName))
            ValidateName(modelAvailabilityName);
    }

    public static void ValidateModelAvailabilitySearchArguments(this ModelAvailabilitySearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.AvailabilityName))
            ValidateString(request.AvailabilityName);
    }

    public static void ValidateModelAvailabilityCreateArguments(this ModelAvailabilityCreateRequest request)
        => ValidateModelAvailabilityName(request.AvailabilityName);

    public static void ValidateModelAvailabilityPatchArguments(this ModelAvailabilityPatchRequest request)
        => ValidateModelAvailabilityName(request.AvailabilityName);
}
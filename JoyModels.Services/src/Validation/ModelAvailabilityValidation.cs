using JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;

namespace JoyModels.Services.Validation;

public static class ModelAvailabilityValidation
{
    private static void ValidateModelAvailabilityName(string modelAvailabilityName)
    {
        if (!string.IsNullOrWhiteSpace(modelAvailabilityName))
            RegularExpressionValidation.ValidateName(modelAvailabilityName);
    }

    public static void ValidateModelAvailabilitySearchArguments(this ModelAvailabilitySearchRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.AvailabilityName))
            RegularExpressionValidation.ValidateText(request.AvailabilityName);
    }

    public static void ValidateModelAvailabilityCreateArguments(this ModelAvailabilityCreateRequest request)
        => ValidateModelAvailabilityName(request.AvailabilityName);

    public static void ValidateModelAvailabilityPatchArguments(this ModelAvailabilityPatchRequest request)
        => ValidateModelAvailabilityName(request.AvailabilityName);
}
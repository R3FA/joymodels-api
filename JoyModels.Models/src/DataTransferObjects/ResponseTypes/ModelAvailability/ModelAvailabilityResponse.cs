namespace JoyModels.Models.DataTransferObjects.ResponseTypes.ModelAvailability;

public class ModelAvailabilityResponse
{
    public Guid Uuid { get; set; }

    public string AvailabilityName { get; set; } = string.Empty;
}
namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelCreateRequest
{
    // TODO: guid ces sam generisat
    // public Guid Uuid { get; set; }

    public string Name { get; set; } = string.Empty;

    // TODO: Iscupaj userUuid iz tokena
    // public Guid UserUuid { get; set; }

    // TODO:I ovo sam
    // public DateTime CreatedAt { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    // TODO: I ovo
    // public Guid ModelAvailabilityUuid { get; set; }
}
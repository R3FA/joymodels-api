namespace JoyModels.Models.DataTransferObjects.CustomReturnTypes;

public class SuccessReturnDetails
{
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Detail { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Instance { get; set; }
}
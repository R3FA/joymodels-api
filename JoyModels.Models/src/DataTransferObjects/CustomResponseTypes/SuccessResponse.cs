namespace JoyModels.Models.DataTransferObjects.CustomResponseTypes;

public class SuccessResponse
{
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Detail { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Instance { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes;

public class SuccessResponse
{
    [Required] public string Type { get; set; } = null!;
    [Required] public string Title { get; set; } = null!;
    [Required] public string Detail { get; set; } = null!;
    [Required] public string Status { get; set; } = null!;
    [Required] public string Instance { get; set; } = null!;
}
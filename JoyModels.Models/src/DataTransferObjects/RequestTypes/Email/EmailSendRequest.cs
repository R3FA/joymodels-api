using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Email;

public class EmailSendRequest
{
    [Required] public string To { get; set; } = string.Empty;
    public string? Subject { get; set; } = string.Empty;
    [Required] public string Body { get; set; } = string.Empty;
}
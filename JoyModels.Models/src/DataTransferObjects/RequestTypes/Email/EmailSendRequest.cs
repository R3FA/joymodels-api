namespace JoyModels.Models.DataTransferObjects.RequestTypes.Email;

public class EmailSendRequest
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
namespace JoyModels.Models.DataTransferObjects.RequestTypes.Email;

public class EmailSendUserDetailsRequest
{
    public string Email { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
    public string OtpExpirationDate { get; set; } = string.Empty;
}
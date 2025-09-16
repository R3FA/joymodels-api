namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoPendingUserCreate
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public string OtpCode { get; set; } = null!;
    public DateTime OtpCreatedAt { get; set; }
    public DateTime OtpExpirationDate { get; set; }
}
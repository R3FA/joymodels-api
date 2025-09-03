using JoyModels.Models.DataTransferObjects.Users;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoGet
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public string OtpCode { get; set; } = null!;
    public DateTime OtpCreatedAt { get; set; }
    public DateTime OtpExpirationDate { get; set; }
    public virtual UserGet User { get; set; } = null!;
}
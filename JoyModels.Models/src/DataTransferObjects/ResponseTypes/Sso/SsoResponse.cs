namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

public class SsoResponse
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public virtual SsoUserResponse User { get; set; } = null!;
}
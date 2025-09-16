namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoReturn
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public virtual SsoUserGet User { get; set; } = null!;
}
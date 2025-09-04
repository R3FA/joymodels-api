using JoyModels.Models.DataTransferObjects.User;

namespace JoyModels.Models.DataTransferObjects.Sso;

public class SsoGet
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public virtual UserGet User { get; set; } = null!;
}
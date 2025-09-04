namespace JoyModels.Models.DataTransferObjects.UserRole;

public class UserRoleGet
{
    public Guid Uuid { get; set; }
    public string RoleName { get; set; } = null!;
}
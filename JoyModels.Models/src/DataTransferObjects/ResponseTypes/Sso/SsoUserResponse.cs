using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;

public class SsoUserResponse
{
    public Guid Uuid { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string NickName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string UserAccessToken { get; set; } = null!;
    public string UserPictureLocation { get; set; } = null!;
    public virtual UserRoleResponse UserRole { get; set; } = null!;
}
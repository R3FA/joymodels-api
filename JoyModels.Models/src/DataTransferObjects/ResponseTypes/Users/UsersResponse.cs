using JoyModels.Models.DataTransferObjects.ResponseTypes.UserRole;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

public class UsersResponse
{
    public Guid Uuid { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string NickName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string UserPictureLocation { get; set; } = null!;
    public int UserFollowers { get; set; }
    public int UserFollowing { get; set; }
    public virtual UserRoleResponse UserRole { get; set; } = null!;
}
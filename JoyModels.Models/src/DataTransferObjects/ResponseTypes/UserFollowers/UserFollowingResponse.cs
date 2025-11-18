using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.UserFollowers;

public class UserFollowingResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse TargetUser { get; set; } = null!;
    public DateTime FollowedAt { get; set; }
}
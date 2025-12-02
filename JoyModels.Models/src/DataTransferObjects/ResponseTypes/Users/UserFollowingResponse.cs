namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

public class UserFollowingResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse TargetUser { get; set; } = null!;
    public DateTime FollowedAt { get; set; }
}
namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

public class UserFollowerResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse OriginUser { get; set; } = null!;
    public DateTime FollowedAt { get; set; }
}
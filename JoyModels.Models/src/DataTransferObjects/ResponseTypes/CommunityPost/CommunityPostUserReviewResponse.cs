using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;

public class CommunityPostUserReviewResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse User { get; set; } = null!;
    public CommunityPostResponse? CommunityPost { get; set; }
    public CommunityPostReviewTypeResponse ReviewType { get; set; } = null!;
}
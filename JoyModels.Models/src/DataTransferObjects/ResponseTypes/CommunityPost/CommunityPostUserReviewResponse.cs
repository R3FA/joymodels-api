using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;

public class CommunityPostUserReviewResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse User { get; set; } = null!;
    public CommunityPostReviewTypeResponse ReviewType { get; set; } = null!;
}
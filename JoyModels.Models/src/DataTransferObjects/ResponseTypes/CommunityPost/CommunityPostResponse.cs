using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;

public class CommunityPostResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse User { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CommunityPostUserReviewResponse UserReview { get; set; } = null!;
    public string? YoutubeVideoLink { get; set; }
}
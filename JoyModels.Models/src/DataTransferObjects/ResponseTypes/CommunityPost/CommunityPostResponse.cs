using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;

public class CommunityPostResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse User { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? YoutubeVideoLink { get; set; }
    public int CommunityPostLikes { get; set; }
    public int CommunityPostDislikes { get; set; }
    public CommunityPostTypeResponse CommunityPostType { get; set; } = null!;
    public List<CommunityPostPictureResponse> PictureLocations { get; set; } = null!;
}
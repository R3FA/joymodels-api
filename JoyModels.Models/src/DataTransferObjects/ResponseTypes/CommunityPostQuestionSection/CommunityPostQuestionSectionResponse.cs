using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;

public class CommunityPostQuestionSectionResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse User { get; set; } = null!;
    public CommunityPostResponse CommunityPost { get; set; } = null!;
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public CommunityPostQuestionSectionParent? ParentMessage { get; set; }
    public List<CommunityPostQuestionSectionReply>? Replies { get; set; }
}

public class CommunityPostQuestionSectionParent
{
    public Guid Uuid { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UsersResponse User { get; set; } = null!;
}

public class CommunityPostQuestionSectionReply
{
    public Guid Uuid { get; set; }
    public string MessageText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public UsersResponse User { get; set; } = null!;
}
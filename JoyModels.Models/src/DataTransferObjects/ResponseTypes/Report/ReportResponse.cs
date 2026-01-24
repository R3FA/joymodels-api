using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Report;

public class ReportResponse
{
    public Guid Uuid { get; set; }
    public UsersResponse Reporter { get; set; } = null!;
    public string ReportedEntityType { get; set; } = string.Empty;
    public Guid ReportedEntityUuid { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public UsersResponse? ReviewedBy { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public UsersResponse? ReportedUser { get; set; }
    public CommunityPostResponse? ReportedCommunityPost { get; set; }
    public CommunityPostQuestionSectionResponse? ReportedCommunityPostComment { get; set; }
    public ModelReviewResponse? ReportedModelReview { get; set; }
    public ModelFaqSectionResponse? ReportedModelFaqQuestion { get; set; }
}
namespace JoyModels.Models.Database.Entities;

public partial class CommunityPostReviewType
{
    public Guid Uuid { get; set; }

    public string ReviewName { get; set; } = null!;

    public virtual ICollection<CommunityPostUserReview> CommunityPostUserReviews { get; set; } =
        new List<CommunityPostUserReview>();
}
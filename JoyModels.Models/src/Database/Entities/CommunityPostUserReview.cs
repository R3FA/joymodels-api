namespace JoyModels.Models.Database.Entities;

public partial class CommunityPostUserReview
{
    public Guid Uuid { get; set; }

    public Guid UserUuid { get; set; }

    public Guid CommunityPostUuid { get; set; }

    public Guid ReviewTypeUuid { get; set; }

    public virtual CommunityPost CommunityPostUu { get; set; } = null!;

    public virtual CommunityPostReviewType ReviewTypeUu { get; set; } = null!;

    public virtual User UserUu { get; set; } = null!;
}
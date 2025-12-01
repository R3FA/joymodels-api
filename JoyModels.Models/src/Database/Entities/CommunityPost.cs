namespace JoyModels.Models.Database.Entities;

public partial class CommunityPost
{
    public Guid Uuid { get; set; }

    public Guid UserUuid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid PostTypeUuid { get; set; }

    public string? YoutubeVideoLink { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CommunityPostLikes { get; set; }
    public int CommunityPostDislikes { get; set; }

    public virtual ICollection<CommunityPostPicture> CommunityPostPictures { get; set; } =
        new List<CommunityPostPicture>();

    public virtual ICollection<CommunityPostQuestionSection> CommunityPostQuestionSections { get; set; } =
        new List<CommunityPostQuestionSection>();

    public virtual ICollection<CommunityPostUserReview> CommunityPostUserReviews { get; set; } =
        new List<CommunityPostUserReview>();

    public virtual CommunityPostType PostTypeUu { get; set; } = null!;

    public virtual User UserUu { get; set; } = null!;
}
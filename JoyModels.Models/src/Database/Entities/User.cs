namespace JoyModels.Models.Database.Entities;

public partial class User
{
    public Guid Uuid { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string NickName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid UserRoleUuid { get; set; }
    public string UserPictureLocation { get; set; } = null!;
    public int UserFollowerCount { get; set; }
    public int UserFollowingCount { get; set; }
    public int UserLikedModelsCount { get; set; }

    public virtual ICollection<CommunityPostQuestionSection> CommunityPostQuestionSectionUserUus { get; set; } =
        new List<CommunityPostQuestionSection>();

    public virtual ICollection<CommunityPostUserReview> CommunityPostUserReviews { get; set; } =
        new List<CommunityPostUserReview>();

    public virtual ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>();

    public virtual ICollection<ModelFaqSection> ModelFaqSectionUserUus { get; set; } =
        new List<ModelFaqSection>();

    public virtual ICollection<ModelReview> ModelReviews { get; set; } = new List<ModelReview>();

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();

    public virtual ICollection<PendingUser> PendingUsers { get; set; } = new List<PendingUser>();

    public virtual ICollection<UserFollower> UserFollowerUserOriginUus { get; set; } = new List<UserFollower>();

    public virtual ICollection<UserFollower> UserFollowerUserTargetUus { get; set; } = new List<UserFollower>();

    public virtual ICollection<UserModelLike> UserModelLikes { get; set; } = new List<UserModelLike>();

    public virtual UserRole UserRoleUu { get; set; } = null!;

    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    public virtual ICollection<ShoppingCart> ShoppingCartItems { get; set; } = new List<ShoppingCart>();
}
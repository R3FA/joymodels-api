namespace JoyModels.Models.Database.Entities;

public partial class CommunityPostQuestionSection
{
    public Guid Uuid { get; set; }

    public Guid? ParentMessageUuid { get; set; }

    public Guid UserUuid { get; set; }

    public Guid CommunityPostUuid { get; set; }

    public string MessageText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual CommunityPost CommunityPostUu { get; set; } = null!;

    public virtual CommunityPostQuestionSection? ParentMessage { get; set; }

    public virtual ICollection<CommunityPostQuestionSection> Replies { get; set; } =
        new List<CommunityPostQuestionSection>();

    public virtual User UserUu { get; set; } = null!;
}
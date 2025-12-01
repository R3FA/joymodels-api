namespace JoyModels.Models.Database.Entities;

public partial class CommunityPostPicture
{
    public Guid Uuid { get; set; }

    public Guid CommunityPostUuid { get; set; }

    public string PictureLocation { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual CommunityPost CommunityPostUu { get; set; } = null!;
}
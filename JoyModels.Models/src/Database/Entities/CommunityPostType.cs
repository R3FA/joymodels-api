namespace JoyModels.Models.Database.Entities;

public partial class CommunityPostType
{
    public Guid Uuid { get; set; }

    public string CommunityPostName { get; set; } = null!;

    public virtual ICollection<CommunityPost> CommunityPosts { get; set; } = new List<CommunityPost>();
}

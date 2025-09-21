namespace JoyModels.Models.Database.Entities;

public partial class UserFollower
{
    public Guid Uuid { get; set; }

    public Guid UserOriginUuid { get; set; }

    public Guid UserTargetUuid { get; set; }

    public DateTime FollowedAt { get; set; }

    public virtual User UserOriginUu { get; set; } = null!;

    public virtual User UserTargetUu { get; set; } = null!;
}

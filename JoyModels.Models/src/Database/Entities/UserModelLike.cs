namespace JoyModels.Models.Database.Entities;

public partial class UserModelLike
{
    public Guid Uuid { get; set; }

    public Guid UserUuid { get; set; }

    public Guid ModelUuid { get; set; }

    public virtual Model ModelUu { get; set; } = null!;

    public virtual User UserUu { get; set; } = null!;
}
namespace JoyModels.Models.Database.Entities;

public partial class Library
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public Guid ModelUuid { get; set; }
    public Guid OrderUuid { get; set; }
    public DateTime AcquiredAt { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual Model Model { get; set; } = null!;
    public virtual Order Order { get; set; } = null!;
}
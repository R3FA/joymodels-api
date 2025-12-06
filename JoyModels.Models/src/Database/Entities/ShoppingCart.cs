namespace JoyModels.Models.Database.Entities;

public partial class ShoppingCart
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public Guid ModelUuid { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual User UserUu { get; set; } = null!;
    public virtual Model ModelUu { get; set; } = null!;
}
namespace JoyModels.Models.Database.Entities;

public partial class Order
{
    public Guid Uuid { get; set; }
    public Guid UserUuid { get; set; }
    public Guid ModelUuid { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StripePaymentIntentId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual Model Model { get; set; } = null!;
}
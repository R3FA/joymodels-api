namespace JoyModels.Models.Database.Entities;

public partial class UserToken
{
    public Guid Uuid { get; set; }

    public Guid UserUuid { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenExpirationDate { get; set; }

    public virtual User UserUu { get; set; } = null!;
}
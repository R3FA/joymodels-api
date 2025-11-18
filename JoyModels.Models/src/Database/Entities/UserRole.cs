namespace JoyModels.Models.Database.Entities;

public partial class UserRole
{
    public Guid Uuid { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
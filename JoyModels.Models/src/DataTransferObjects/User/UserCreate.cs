namespace JoyModels.Models.DataTransferObjects.User;

public class UserCreate
{
    public Guid Uuid { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string NickName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string UserRoleUuid { get; set; } = null!;
}
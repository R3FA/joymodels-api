namespace JoyModels.Models.DataTransferObjects.Users;

public class UserGet
{
    public Guid Uuid { get; set; }
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string NickName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string UserRoleUuid { get; set; } = null!;
}
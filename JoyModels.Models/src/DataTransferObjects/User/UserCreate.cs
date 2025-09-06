namespace JoyModels.Models.DataTransferObjects.User;

public class UserCreate
{
    public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    public string Nickname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.User;

public class UserCreate
{
    [Required] public string FirstName { get; set; } = null!;
    public string? LastName { get; set; }
    [Required] public string Nickname { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
}
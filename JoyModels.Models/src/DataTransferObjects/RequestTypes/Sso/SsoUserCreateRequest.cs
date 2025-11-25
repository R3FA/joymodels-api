using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Sso;

public class SsoUserCreateRequest
{
    [Required, MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
    public string? LastName { get; set; }

    [Required, MaxLength(50, ErrorMessage = "Nickname cannot exceed 50 characters.")]
    public string Nickname { get; set; } = string.Empty;

    [Required, MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(50, ErrorMessage = "Password cannot exceed 50 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required] public IFormFile UserPicture { get; set; } = null!;
}
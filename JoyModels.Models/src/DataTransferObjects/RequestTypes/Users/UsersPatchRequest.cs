using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Users;

public class UsersPatchRequest
{
    public Guid UserUuid { get; set; }

    [MaxLength(100, ErrorMessage = "FirstName cannot exceed 100 characters.")]
    public string? FirstName { get; set; }

    [MaxLength(100, ErrorMessage = "LastName cannot exceed 100 characters.")]
    public string? LastName { get; set; }

    [MaxLength(50, ErrorMessage = "Nickname cannot exceed 50 characters.")]
    public string? Nickname { get; set; }

    public IFormFile? UserPicture { get; set; }
}
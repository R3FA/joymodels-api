using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Models;

public class ModelResponse
{
    [Required] public Guid Uuid { get; set; }

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public Guid UserUuid { get; set; }

    [Required] public DateTime CreatedAt { get; set; }

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public decimal Price { get; set; }

    [Required] public Guid ModelAvailabilityUuid { get; set; }

    [Required] public virtual UsersResponse User { get; set; } = null!;
}
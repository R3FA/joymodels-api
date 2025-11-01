using JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelAvailability;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.Models;

public class ModelResponse
{
    public Guid Uuid { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid UserUuid { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public virtual UsersResponse User { get; set; } = null!;
    public virtual ModelAvailabilityResponse ModelAvailability { get; set; } = null!;
    public virtual ICollection<CategoryResponse> ModelCategories { get; set; } = [];
}
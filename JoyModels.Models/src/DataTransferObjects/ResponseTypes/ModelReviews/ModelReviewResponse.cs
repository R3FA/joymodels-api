using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;

public class ModelReviewResponse
{
    public Guid Uuid { get; set; }
    public ModelResponse ModelResponse { get; set; } = null!;
    public UsersResponse UsersResponse { get; set; } = null!;
    public ModelReviewTypeResponse ModelReviewTypeResponse { get; set; } = null!;
    public string ModelReviewText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
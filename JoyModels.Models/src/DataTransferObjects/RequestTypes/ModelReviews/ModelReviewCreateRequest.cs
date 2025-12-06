using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;

public class ModelReviewCreateRequest
{
    [Required] public Guid ModelUuid { get; set; }
    [Required] public Guid ModelReviewTypeUuid { get; set; }

    [Required, MaxLength(5000, ErrorMessage = "ModelReviewText cannot exceed 5000 characters.")]
    public string ModelReviewText { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;

public class ModelReviewPatchRequest
{
    [Required] public Guid ModelReviewUuid { get; set; }
    public Guid? ModelReviewTypeUuid { get; set; }

    [MaxLength(5000, ErrorMessage = "ModelReviewText cannot exceed 5000 characters.")]
    public string? ModelReviewText { get; set; }
}
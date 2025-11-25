using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;

public class ModelReviewPatchRequest
{
    [Required] public Guid ModelReviewUuid { get; set; }
    public Guid? ModelReviewTypeUuid { get; set; }

    [MaxLength(1500, ErrorMessage = "ModelReviewText cannot exceed 1500 characters.")]
    public string? ModelReviewText { get; set; }
}
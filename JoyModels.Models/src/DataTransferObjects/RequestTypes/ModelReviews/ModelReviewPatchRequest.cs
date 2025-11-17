using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;

public class ModelReviewPatchRequest
{
    [Required] public Guid ModelReviewUuid { get; set; }
    public Guid? ModelReviewTypeUuid { get; set; }
    public string? ModelReviewText { get; set; }
}
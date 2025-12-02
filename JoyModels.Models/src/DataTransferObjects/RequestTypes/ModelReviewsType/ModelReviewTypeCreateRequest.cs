using System.ComponentModel.DataAnnotations;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;

public class ModelReviewTypeCreateRequest
{
    [Required, MaxLength(50, ErrorMessage = "ModelReviewTypeName cannot exceed 50 characters.")]
    public string ModelReviewTypeName { get; set; } = string.Empty;
}
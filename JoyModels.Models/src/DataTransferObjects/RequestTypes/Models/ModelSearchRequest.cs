using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelSearchRequest : PaginationRequest
{
    [MaxLength(100, ErrorMessage = "ModelName cannot exceed 100 characters.")]
    public string? ModelName { get; set; }

    [DefaultValue(false)] public bool ArePrivateUserModelsSearched { get; set; }
}
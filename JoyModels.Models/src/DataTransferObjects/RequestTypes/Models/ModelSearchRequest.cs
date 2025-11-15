using System.ComponentModel;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.Models;

public class ModelSearchRequest : PaginationRequest
{
    public string? ModelName { get; set; } = string.Empty;
    [DefaultValue(false)] public bool ArePrivateUserModelsSearched { get; set; }
}
using System.ComponentModel;
using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;

public class ModelFaqSectionSearchRequest : PaginationRequest
{
    public Guid ModelUuid { get; set; }
    public string? FaqMessage { get; set; }
    [DefaultValue(false)] public bool IsMyFaqSectionFiltered { get; set; }
}
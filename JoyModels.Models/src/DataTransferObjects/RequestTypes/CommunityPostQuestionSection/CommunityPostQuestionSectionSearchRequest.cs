using JoyModels.Models.DataTransferObjects.RequestTypes.Pagination;

namespace JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;

public class CommunityPostQuestionSectionSearchRequest : PaginationRequest
{
    public Guid CommunityPostUuid { get; set; }
}
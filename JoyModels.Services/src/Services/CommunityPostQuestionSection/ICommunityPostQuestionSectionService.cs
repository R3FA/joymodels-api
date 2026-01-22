using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.CommunityPostQuestionSection;

public interface ICommunityPostQuestionSectionService
{
    Task<CommunityPostQuestionSectionResponse> GetByUuid(Guid communityPostQuestionSectionUuid);

    Task<PaginationResponse<CommunityPostQuestionSectionResponse>> Search(
        CommunityPostQuestionSectionSearchRequest request);

    Task<CommunityPostQuestionSectionResponse> Create(CommunityPostQuestionSectionCreateRequest request);
    Task<CommunityPostQuestionSectionResponse> CreateAnswer(CommunityPostQuestionSectionCreateAnswerRequest request);
    Task<CommunityPostQuestionSectionResponse> Patch(CommunityPostQuestionSectionPatchRequest request);
    Task Delete(Guid communityPostQuestionSectionUuid);
}
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.ModelFaqSection;

public interface IModelFaqSectionService
{
    Task<ModelFaqSectionResponse> GetByUuid(Guid modelFaqSectionUuid);
    Task<PaginationResponse<ModelFaqSectionResponse>> Search(ModelFaqSectionSearchRequest request);
    Task<ModelFaqSectionResponse> Create(ModelFaqSectionCreateRequest request);
    Task<ModelFaqSectionResponse> CreateAnswer(ModelFaqSectionCreateAnswerRequest request);
    Task<ModelFaqSectionResponse> Patch(ModelFaqSectionPatchRequest request);
    Task Delete(ModelFaqSectionDeleteRequest request);
}
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;

namespace JoyModels.Services.Services.ModelFaqSection;

public interface IModelFaqSectionService
{
    Task<ModelFaqSectionResponse> GetByUuid(Guid modelFaqSectionUuid);
    Task<ModelFaqSectionResponse> Create(ModelFaqSectionCreateRequest request);
    Task<ModelFaqSectionResponse> CreateAnswer(ModelFaqSectionCreateAnswerRequest request);
    Task<ModelFaqSectionResponse> Patch(ModelFaqSectionPatchRequest request);
    Task Delete(ModelFaqSectionDeleteRequest request);
}
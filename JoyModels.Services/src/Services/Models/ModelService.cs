using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Services.Services.Models.HelperMethods;
using JoyModels.Services.Validation.Models;

namespace JoyModels.Services.Services.Models;

public class ModelService(JoyModelsDbContext context, IMapper mapper) : IModelService
{
    public async Task<ModelResponse> GetByUuid(Guid modelUuid)
    {
        var modelEntity = await ModelHelperMethods.GetModelEntity(context, modelUuid);
        return mapper.Map<ModelResponse>(modelEntity);
    }

    public async Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request)
    {
        request.ValidateModelSearchArguments();

        var modelEntities = await ModelHelperMethods.SearchModelEntities(context, request);

        return mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);
    }

    // TODO: Implementirati prvo ModelCategories, ModelFaqSections, ModelReviews, UserModelLikes pa tek onda ovo
    // TODO: Takodjer namjesti ModelCreateRequest DTO
    public Task<ModelResponse> Create(ModelCreateRequest request)
    {
        throw new NotImplementedException();
    }
}
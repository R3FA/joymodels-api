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
        var modelResponse = mapper.Map<ModelResponse>(modelEntity);

        return modelResponse;
    }

    public async Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request)
    {
        request.ValidateModelSearchArguments();

        var modelEntities = await ModelHelperMethods.SearchModelEntities(context, request);
        var modelsResponse = mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);

        return modelsResponse;
    }
}
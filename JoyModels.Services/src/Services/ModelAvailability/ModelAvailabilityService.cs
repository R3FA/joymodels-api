using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelAvailability;
using JoyModels.Services.Services.ModelAvailability.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.ModelAvailability;

namespace JoyModels.Services.Services.ModelAvailability;

public class ModelAvailabilityService(JoyModelsDbContext context, IMapper mapper, UserAuthValidation userAuthValidation)
    : IModelAvailabilityService
{
    public async Task<ModelAvailabilityResponse> GetByUuid(Guid modelAvailabilityUuid)
    {
        var modelAvailabilityEntity =
            await ModelAvailabilityHelperMethods.GetModelAvailabilityEntity(context, modelAvailabilityUuid);
        return mapper.Map<ModelAvailabilityResponse>(modelAvailabilityEntity);
    }

    public async Task<PaginationResponse<ModelAvailabilityResponse>> Search(ModelAvailabilitySearchRequest request)
    {
        request.ValidateModelAvailabilitySearchArguments();

        var modelAvailabilityEntities =
            await ModelAvailabilityHelperMethods.SearchModelAvailabilityEntities(context, request);

        return mapper.Map<PaginationResponse<ModelAvailabilityResponse>>(modelAvailabilityEntities);
    }

    public async Task<ModelAvailabilityResponse> Create(ModelAvailabilityCreateRequest request)
    {
        request.ValidateModelAvailabilityCreateArguments();

        var modelAvailabilityEntity = mapper.Map<JoyModels.Models.Database.Entities.ModelAvailability>(request);
        await modelAvailabilityEntity.CreateModelAvailability(context);

        return mapper.Map<ModelAvailabilityResponse>(modelAvailabilityEntity);
    }

    public async Task<ModelAvailabilityResponse> Patch(Guid modelAvailabilityUuid,
        ModelAvailabilityPatchRequest request)
    {
        userAuthValidation.ValidateRequestUuids(modelAvailabilityUuid, request.Uuid);
        request.ValidateModelAvailabilityPatchArguments();

        await request.PatchModelAvailability(context);

        return mapper.Map<ModelAvailabilityResponse>(request);
    }

    public async Task Delete(Guid modelAvailabilityUuid)
    {
        await ModelAvailabilityHelperMethods.DeleteModelAvailability(context, modelAvailabilityUuid);
    }
}
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelAvailability;

namespace JoyModels.Services.Services.ModelAvailability;

public interface IModelAvailabilityService
{
    Task<ModelAvailabilityResponse> GetByUuid(Guid modelAvailabilityUuid);
    Task<PaginationResponse<ModelAvailabilityResponse>> Search(ModelAvailabilitySearchRequest request);
    Task<ModelAvailabilityResponse> Create(ModelAvailabilityCreateRequest request);
    Task<ModelAvailabilityResponse> Patch(Guid modelAvailabilityUuid, ModelAvailabilityPatchRequest request);
    Task Delete(Guid modelAvailabilityUuid);
}
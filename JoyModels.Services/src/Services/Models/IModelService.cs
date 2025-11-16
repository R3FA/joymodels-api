using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.Models;

public interface IModelService
{
    Task<ModelResponse> GetByUuid(ModelGetByUuidRequest request);
    Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request);
    Task<PaginationResponse<ModelResponse>> AdminSearch(ModelAdminSearchRequest request);
    Task<ModelResponse> Create(ModelCreateRequest request);
    Task<ModelResponse> Patch(Guid modelUuid, ModelPatchRequest request);
    Task Delete(Guid modelUuid);
}
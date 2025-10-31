using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.Models;

public interface IModelService
{
    Task<ModelResponse> GetByUuid(Guid modelUuid);
    Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request);
    Task<ModelResponse> Create(ModelCreateRequest request);
}
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Services.Services.Models;

public interface IModelService
{
    Task<ModelResponse> GetByUuid(Guid modelUuid);
    Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request);
    Task<ModelResponse> Create(ModelCreateRequest request);
    Task<string> SaveFile(IFormFile file, Guid modelUuid);
}
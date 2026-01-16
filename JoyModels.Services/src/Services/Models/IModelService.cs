using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Core;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.Models;

public interface IModelService
{
    Task<ModelResponse> GetByUuid(ModelGetByUuidRequest request);
    Task<PictureResponse> GetModelPictures(Guid modelUuid, string modelPictureLocationPath);
    Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request);
    Task<PaginationResponse<ModelResponse>> AdminSearch(ModelAdminSearchRequest request);
    Task<bool> IsModelLiked(Guid modelUuid);
    Task<ModelResponse> Create(ModelCreateRequest request);
    Task ModelLike(Guid modelUuid);
    Task<ModelResponse> Patch(ModelPatchRequest request);
    Task ModelUnlike(Guid modelUuid);
    Task Delete(Guid modelUuid);
}
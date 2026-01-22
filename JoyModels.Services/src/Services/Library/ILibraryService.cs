using JoyModels.Models.DataTransferObjects.RequestTypes.Library;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Library;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.Library;

public interface ILibraryService
{
    Task<LibraryResponse> GetByUuid(Guid libraryUuid);
    Task<LibraryResponse> GetByModelUuid(Guid modelUuid);
    Task<PaginationResponse<LibraryResponse>> Search(LibrarySearchRequest request);
    Task<bool> HasUserPurchasedModel(Guid modelUuid);
    Task<string> GetModelDownloadPath(Guid modelUuid);
}
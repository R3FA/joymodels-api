using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Services.Services.Users;

public interface IUsersService
{
    Task<UsersResponse> GetByUuid(Guid userUuid);
    Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request);
    Task<UsersResponse> Patch(Guid userUuid, UsersPatchRequest request);
    Task Delete(Guid userUuid);
}
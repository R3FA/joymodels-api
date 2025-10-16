using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Services.Services.Users;

public interface IUsersService
{
    Task<UsersResponse> GetByUuid(Guid userUuid);
    Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request);
}
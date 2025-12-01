using JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserRole;

namespace JoyModels.Services.Services.UserRole;

public interface IUserRoleService
{
    Task<UserRoleResponse> GetByUuid(Guid userRoleUuid);
    Task<PaginationResponse<UserRoleResponse>> Search(UserRoleSearchRequest request);
}
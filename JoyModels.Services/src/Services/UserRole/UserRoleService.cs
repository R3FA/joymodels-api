using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserRole;
using JoyModels.Services.Services.UserRole.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.UserRole;

public class UserRoleService(
    JoyModelsDbContext context,
    IMapper mapper) : IUserRoleService
{
    public async Task<UserRoleResponse> GetByUuid(Guid userRoleUuid)
    {
        var userRoleEntity = await UserRoleHelperMethods.GetUserRoleEntity(context, userRoleUuid);
        return mapper.Map<UserRoleResponse>(userRoleEntity);
    }

    public async Task<PaginationResponse<UserRoleResponse>> Search(UserRoleSearchRequest request)
    {
        request.ValidateUserRoleSearchArguments();

        var userRoleEntities = await UserRoleHelperMethods.SearchUserRoleEntities(context, request);

        return mapper.Map<PaginationResponse<UserRoleResponse>>(userRoleEntities);
    }

    public async Task<UserRoleResponse> Create(UserRoleCreateRequest request)
    {
        request.ValidateUserRoleCreateArguments();

        var userRoleEntity = mapper.Map<JoyModels.Models.Database.Entities.UserRole>(request);
        await userRoleEntity.CreateUserRoleEntity(context);

        return await GetByUuid(userRoleEntity.Uuid);
    }

    public async Task<UserRoleResponse> Patch(UserRolePatchRequest request)
    {
        request.ValidateUserRolePatchArguments();

        await request.PatchUserRoleEntity(context);

        return await GetByUuid(request.RoleUuid);
    }

    public async Task Delete(Guid userRoleUuid)
    {
        await UserRoleHelperMethods.DeleteUserRole(context, userRoleUuid);
    }
}
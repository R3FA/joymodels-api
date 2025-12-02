using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.UserRole.HelperMethods;

public static class UserRoleHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.UserRole> GetUserRoleEntity(
        JoyModelsDbContext context,
        Guid userRoleUuid)
    {
        var userRoleEntity = await context.UserRoles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid == userRoleUuid);

        return userRoleEntity ?? throw new KeyNotFoundException("User role with sent values is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.UserRole>> SearchUserRoleEntities(
        JoyModelsDbContext context,
        UserRoleSearchRequest request)
    {
        var baseQuery = context.UserRoles
            .AsNoTracking();

        var filteredQuery = request.RoleName switch
        {
            not null => baseQuery.Where(x => x.RoleName.Contains(request.RoleName)),
            _ => baseQuery
        };

        filteredQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.UserRole>.OrderBy(filteredQuery, request.OrderBy);

        var userRoleEntities = await PaginationBase<JoyModels.Models.Database.Entities.UserRole>.CreateAsync(
            filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return userRoleEntities;
    }

    public static async Task CreateUserRoleEntity(this JoyModels.Models.Database.Entities.UserRole userRoleEntity,
        JoyModelsDbContext context)
    {
        await context.UserRoles.AddAsync(userRoleEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchUserRoleEntity(this UserRolePatchRequest request, JoyModelsDbContext context)
    {
        var totalRecords = await context.UserRoles
            .Where(x => x.Uuid == request.RoleUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.RoleName,
                z => request.RoleName));

        if (totalRecords <= 0)
            throw new KeyNotFoundException("User role with sent values is not found for patching.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteUserRole(JoyModelsDbContext context, Guid userRoleUuid)
    {
        var totalRecords = await context.UserRoles
            .Where(x => x.Uuid == userRoleUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"UserRole with UUID `{userRoleUuid}` does not exist.");
    }
}
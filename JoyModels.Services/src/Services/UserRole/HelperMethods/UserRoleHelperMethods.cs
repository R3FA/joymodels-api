using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
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
}
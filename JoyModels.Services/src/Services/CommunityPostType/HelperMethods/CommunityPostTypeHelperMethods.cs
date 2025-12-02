using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.CommunityPostType.HelperMethods;

public static class CommunityPostTypeHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.CommunityPostType> GetCommunityPostTypeEntity(
        JoyModelsDbContext context, Guid communityPostTypeUuid)
    {
        var communityPostTypeEntity = await context.CommunityPostTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid == communityPostTypeUuid);

        return communityPostTypeEntity ??
               throw new KeyNotFoundException("CommunityPostType with sent values is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.CommunityPostType>>
        SearchCommunityPostTypeEntities(
            JoyModelsDbContext context,
            CommunityPostTypeSearchRequest request)
    {
        var baseQuery = context.CommunityPostTypes
            .AsNoTracking();

        var filteredQuery = request.PostTypeName switch
        {
            not null => baseQuery.Where(x => x.CommunityPostName.Contains(request.PostTypeName)),
            _ => baseQuery
        };

        filteredQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.CommunityPostType>.OrderBy(filteredQuery,
                request.OrderBy);

        var communityPostTypeEntities = await PaginationBase<JoyModels.Models.Database.Entities.CommunityPostType>
            .CreateAsync(filteredQuery,
                request.PageNumber,
                request.PageSize,
                request.OrderBy);

        return communityPostTypeEntities;
    }

    public static async Task CreateCommunityPostTypeEntity(
        this JoyModels.Models.Database.Entities.CommunityPostType communityPostTypeEntity, JoyModelsDbContext context)
    {
        await context.CommunityPostTypes.AddAsync(communityPostTypeEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchCommunityPostTypeEntity(this CommunityPostTypePatchRequest request,
        JoyModelsDbContext context)
    {
        var totalRecords = await context.CommunityPostTypes
            .Where(x => x.Uuid == request.PostTypeUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.CommunityPostName,
                z => request.PostTypeName));

        if (totalRecords <= 0)
            throw new KeyNotFoundException("CommunityPostType with sent values is not found for patching.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteCommunityPostTypeEntity(JoyModelsDbContext context, Guid communityPostTypeUuid)
    {
        var totalRecords = await context.CommunityPostTypes
            .Where(x => x.Uuid == communityPostTypeUuid)
            .ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"CommunityPostType with UUID `{communityPostTypeUuid}` does not exist.");

        await context.SaveChangesAsync();
    }
}
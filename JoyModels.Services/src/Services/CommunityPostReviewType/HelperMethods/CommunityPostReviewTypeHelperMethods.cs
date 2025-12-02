using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.CommunityPostReviewType.HelperMethods;

public static class CommunityPostReviewTypeHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.CommunityPostReviewType>
        GetCommunityPostReviewTypeEntity(
            JoyModelsDbContext context, Guid communityPostReviewTypeUuid)
    {
        var communityPostReviewTypeEntity = await context.CommunityPostReviewTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid == communityPostReviewTypeUuid);

        return communityPostReviewTypeEntity ??
               throw new KeyNotFoundException("CommunityPostReviewType with sent values is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.CommunityPostReviewType>>
        SearchCommunityPostReviewTypeEntities(JoyModelsDbContext context,
            CommunityPostReviewTypeSearchRequest request)
    {
        var baseQuery = context.CommunityPostReviewTypes
            .AsNoTracking();

        var filteredQuery = request.CommunityPostReviewTypeName switch
        {
            not null => baseQuery.Where(x => x.ReviewName.Contains(request.CommunityPostReviewTypeName)),
            _ => baseQuery
        };

        filteredQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.CommunityPostReviewType>.OrderBy(filteredQuery,
                request.OrderBy);

        var communityPostReviewTypeEntities =
            await PaginationBase<JoyModels.Models.Database.Entities.CommunityPostReviewType>.CreateAsync(filteredQuery,
                request.PageNumber,
                request.PageSize,
                request.OrderBy);

        return communityPostReviewTypeEntities;
    }

    public static async Task CreateCommunityPostReviewTypeEntity(
        this JoyModels.Models.Database.Entities.CommunityPostReviewType communityPostReviewTypeEntity,
        JoyModelsDbContext context)
    {
        await context.CommunityPostReviewTypes.AddAsync(communityPostReviewTypeEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchCommunityPostReviewTypeEntity(
        this CommunityPostReviewTypePatchRequest request, JoyModelsDbContext context)
    {
        var totalRecords = await context.CommunityPostReviewTypes
            .Where(x => x.Uuid == request.CommunityPostReviewTypeUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.ReviewName,
                z => request.CommunityPostReviewTypeName));

        if (totalRecords <= 0)
            throw new KeyNotFoundException("CommunityPostReviewType with sent values is not found for patching.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteCommunityPostReviewTypeEntity(JoyModelsDbContext context,
        Guid communityPostReviewTypeUuid)
    {
        var totalRecords = await context.CommunityPostReviewTypes
            .Where(x => x.Uuid == communityPostReviewTypeUuid)
            .ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"CommunityPostReviewType with UUID `{communityPostReviewTypeUuid}` does not exist.");

        await context.SaveChangesAsync();
    }
}
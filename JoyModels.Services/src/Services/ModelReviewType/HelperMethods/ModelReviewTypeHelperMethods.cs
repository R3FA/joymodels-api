using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.ModelReviewType.HelperMethods;

public static class ModelReviewTypeHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.ModelReviewType> GetModelReviewTypeEntity(
        JoyModelsDbContext context, Guid modelReviewTypeUuid)
    {
        var modelReviewTypeEntity = await context.ModelReviewTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid == modelReviewTypeUuid);

        return modelReviewTypeEntity ??
               throw new KeyNotFoundException("Model review type with sent values is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.ModelReviewType>>
        SearchModelReviewTypeEntities(
            JoyModelsDbContext context,
            ModelReviewTypeSearchRequest request)
    {
        var baseQuery = context.ModelReviewTypes
            .AsNoTracking();

        var filteredQuery = request.ModelReviewTypeName switch
        {
            not null => baseQuery.Where(x => x.ReviewName.Contains(request.ModelReviewTypeName)),
            _ => baseQuery
        };

        filteredQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.ModelReviewType>.OrderBy(filteredQuery,
                request.OrderBy);

        var modelReviewTypeEntities =
            await PaginationBase<JoyModels.Models.Database.Entities.ModelReviewType>.CreateAsync(
                filteredQuery,
                request.PageNumber,
                request.PageSize,
                request.OrderBy);

        return modelReviewTypeEntities;
    }

    public static async Task CreateModelReviewTypeEntity(
        this JoyModels.Models.Database.Entities.ModelReviewType modelReviewTypeEntity, JoyModelsDbContext context)
    {
        await context.ModelReviewTypes.AddAsync(modelReviewTypeEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchModelReviewTypeEntity(this ModelReviewTypePatchRequest request,
        JoyModelsDbContext context)
    {
        var totalRecords = await context.ModelReviewTypes
            .Where(x => x.Uuid == request.ModelReviewTypeUuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.ReviewName,
                z => request.ModelReviewTypeName));

        if (totalRecords <= 0)
            throw new KeyNotFoundException("Model review type with sent values is not found for patching.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteModelReviewTypeEntity(JoyModelsDbContext context, Guid modelReviewTypeUuid)
    {
        var totalRecords = await context.ModelReviewTypes
            .Where(x => x.Uuid == modelReviewTypeUuid)
            .ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"Model review type with UUID `{modelReviewTypeUuid}` does not exist.");

        await context.SaveChangesAsync();
    }
}
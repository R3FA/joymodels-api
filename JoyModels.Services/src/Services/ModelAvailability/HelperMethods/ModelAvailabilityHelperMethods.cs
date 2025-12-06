using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelAvailability;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.ModelAvailability.HelperMethods;

public static class ModelAvailabilityHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.ModelAvailability> GetModelAvailabilityEntity(
        JoyModelsDbContext context, Guid modelAvailabilityUuid)
    {
        var modelAvailabilityEntity = await context.ModelAvailabilities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid == modelAvailabilityUuid);

        return modelAvailabilityEntity ??
               throw new KeyNotFoundException("Model availability with sent values is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.ModelAvailability>>
        SearchModelAvailabilityEntities(JoyModelsDbContext context, ModelAvailabilitySearchRequest request)
    {
        var baseQuery = context.ModelAvailabilities
            .AsNoTracking();

        var filteredQuery = request.AvailabilityName switch
        {
            not null => baseQuery.Where(x =>
                x.AvailabilityName.Contains(request.AvailabilityName)),
            _ => baseQuery
        };

        filteredQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.ModelAvailability>.OrderBy(filteredQuery,
                request.OrderBy);

        return await PaginationBase<JoyModels.Models.Database.Entities.ModelAvailability>.CreateAsync(filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);
    }

    public static async Task CreateModelAvailability(
        this JoyModels.Models.Database.Entities.ModelAvailability modelAvailabilityEntity, JoyModelsDbContext context)
    {
        await context.ModelAvailabilities.AddAsync(modelAvailabilityEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchModelAvailability(this ModelAvailabilityPatchRequest request,
        JoyModelsDbContext context)
    {
        var totalRecords = await context.ModelAvailabilities
            .Where(x => x.Uuid == request.Uuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.AvailabilityName,
                z => request.AvailabilityName));

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"Model availability with UUID `{request.Uuid}` does not exist.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteModelAvailability(JoyModelsDbContext context, Guid modelAvailabilityUuid)
    {
        var totalRecords = await context.ModelAvailabilities
            .Where(x => x.Uuid == modelAvailabilityUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"Model availability with UUID `{modelAvailabilityUuid}` does not exist.");
    }
}
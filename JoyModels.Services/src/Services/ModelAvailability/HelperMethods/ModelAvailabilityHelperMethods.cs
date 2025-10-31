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
        SearchModelAvailabilityEntities(JoyModelsDbContext context,
            ModelAvailabilitySearchRequest modelAvailabilitySearchRequestDto)
    {
        var baseQuery = context.ModelAvailabilities
            .AsNoTracking();

        var filteredQuery = modelAvailabilitySearchRequestDto.AvailabilityName switch
        {
            not null => baseQuery.Where(x =>
                x.AvailabilityName.Contains(modelAvailabilitySearchRequestDto.AvailabilityName)),
            _ => baseQuery
        };

        filteredQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.ModelAvailability>.OrderBy(filteredQuery,
                modelAvailabilitySearchRequestDto.OrderBy);

        return await PaginationBase<JoyModels.Models.Database.Entities.ModelAvailability>.CreateAsync(filteredQuery,
            modelAvailabilitySearchRequestDto.PageNumber,
            modelAvailabilitySearchRequestDto.PageSize,
            modelAvailabilitySearchRequestDto.OrderBy);
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
        await context.ModelAvailabilities
            .Where(x => x.Uuid == request.Uuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.AvailabilityName,
                z => request.AvailabilityName));
        await context.SaveChangesAsync();
    }

    public static async Task DeleteModelAvailability(JoyModelsDbContext context, Guid modelAvailabilityUuid)
    {
        var numberOfDeletedRows = await context.ModelAvailabilities
            .Where(x => x.Uuid == modelAvailabilityUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();

        if (numberOfDeletedRows == 0)
            throw new KeyNotFoundException(
                $"Model availability with UUID `{modelAvailabilityUuid}` does not exist.");
    }
}
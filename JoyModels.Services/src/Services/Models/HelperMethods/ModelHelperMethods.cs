using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Models.HelperMethods;

public static class ModelHelperMethods
{
    public static async Task<Model> GetModelEntity(JoyModelsDbContext context, Guid modelUuid)
    {
        var modelEntity = await context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .FirstOrDefaultAsync(x => x.Uuid == modelUuid);

        return modelEntity ?? throw new KeyNotFoundException("3D model with sent values is not found.");
    }

    public static async Task<PaginationBase<Model>> SearchModelEntities(JoyModelsDbContext context,
        ModelSearchRequest modelSearchRequestDto)
    {
        var baseQuery = context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu);

        var filteredQuery = modelSearchRequestDto.ModelName switch
        {
            not null => baseQuery.Where(x => x.Name.Contains(modelSearchRequestDto.ModelName)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<Model>.OrderBy(filteredQuery, modelSearchRequestDto.OrderBy);

        var modelEntities = await PaginationBase<Model>.CreateAsync(filteredQuery,
            modelSearchRequestDto.PageNumber,
            modelSearchRequestDto.PageSize,
            modelSearchRequestDto.OrderBy);

        return modelEntities;
    }

    public static async Task CreateModelEntity(this Model modelEntity, JoyModelsDbContext context)
    {
        await context.Models.AddAsync(modelEntity);
        await context.SaveChangesAsync();
    }

    public static async Task CreateModelCategories(this Model modelEntity, JoyModelsDbContext context,
        ModelCreateRequest request)
    {
        foreach (var modelCategoryUuid in request.ModelCategoryUuids)
        {
            var modelCategoryEntity = new ModelCategory
            {
                Uuid = Guid.NewGuid(),
                ModelUuid = modelEntity.Uuid,
                CategoryUuid = modelCategoryUuid
            };

            await context.ModelCategories.AddAsync(modelCategoryEntity);
        }

        await context.SaveChangesAsync();
    }
}
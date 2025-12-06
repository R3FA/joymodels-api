using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Categories.HelperMethods;

public static class CategoryHelperMethods
{
    public static async Task<Category> GetCategoryEntity(JoyModelsDbContext context, Guid categoryUuid)
    {
        var categoryEntity = await context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Uuid == categoryUuid);

        return categoryEntity ?? throw new KeyNotFoundException($"Category with UUID `{categoryUuid}` does not exist.");
    }

    public static async Task<PaginationBase<Category>> SearchCategoryEntities(JoyModelsDbContext context,
        CategorySearchRequest request)
    {
        var baseQuery = context.Categories
            .AsNoTracking();

        var filteredQuery = request.CategoryName switch
        {
            not null => baseQuery.Where(x => x.CategoryName.Contains(request.CategoryName)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<Category>.OrderBy(filteredQuery, request.OrderBy);

        var categoryEntities = await PaginationBase<Category>.CreateAsync(filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return categoryEntities;
    }

    public static async Task CreateCategoryEntity(this Category categoryEntity, JoyModelsDbContext context)
    {
        await context.Categories.AddAsync(categoryEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchCategoryEntity(this CategoryPatchRequest request, JoyModelsDbContext context)
    {
        await context.Categories
            .Where(x => x.Uuid == request.Uuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.CategoryName,
                z => request.CategoryName));
        await context.SaveChangesAsync();
    }

    public static async Task DeleteCategoryEntity(JoyModelsDbContext context, Guid categoryUuid)
    {
        var totalRecords = await context.Categories
            .Where(x => x.Uuid == categoryUuid)
            .ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException(
                $"Category with UUID `{categoryUuid}` does not exist.");

        await context.SaveChangesAsync();
    }
}
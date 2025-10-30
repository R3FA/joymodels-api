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

        return categoryEntity ?? throw new KeyNotFoundException("Category with sent values is not found.");
    }

    public static async Task<PaginationBase<Category>> SearchCategoryEntities(JoyModelsDbContext context,
        CategorySearchRequest categorySearchRequestDto)
    {
        var baseQuery = context.Categories
            .AsNoTracking();

        var filteredQuery = categorySearchRequestDto.CategoryName switch
        {
            not null => baseQuery.Where(x => x.CategoryName.Contains(categorySearchRequestDto.CategoryName)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<Category>.OrderBy(filteredQuery, categorySearchRequestDto.OrderBy);

        var categoryEntities = await PaginationBase<Category>.CreateAsync(filteredQuery,
            categorySearchRequestDto.PageNumber,
            categorySearchRequestDto.PageSize,
            categorySearchRequestDto.OrderBy);

        return categoryEntities;
    }

    public static async Task CreateCategory(this Category categoryEntity, JoyModelsDbContext context)
    {
        await context.Categories.AddAsync(categoryEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchCategory(this CategoryPatchRequest request, JoyModelsDbContext context)
    {
        await context.Categories
            .Where(x => x.Uuid == request.Uuid)
            .ExecuteUpdateAsync(y => y.SetProperty(z => z.CategoryName,
                z => request.CategoryName));
        await context.SaveChangesAsync();
    }

    public static async Task DeleteCategory(JoyModelsDbContext context, Guid categoryUuid)
    {
        var numberOfDeletedRows = await context.Categories
            .Where(x => x.Uuid == categoryUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();

        if (numberOfDeletedRows == 0)
            throw new KeyNotFoundException(
                $"Category with UUID `{categoryUuid}` does not exist.");
    }
}
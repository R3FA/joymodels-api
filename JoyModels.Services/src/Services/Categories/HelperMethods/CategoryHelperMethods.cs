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
}
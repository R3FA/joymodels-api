using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;
using JoyModels.Services.Services.Categories.HelperMethods;
using JoyModels.Services.Validation.Categories;

namespace JoyModels.Services.Services.Categories;

public class CategoryService(JoyModelsDbContext context, IMapper mapper) : ICategoryService
{
    public async Task<CategoryResponse> GetByUuid(Guid categoryUuid)
    {
        var categoryEntity = await CategoryHelperMethods.GetCategoryEntity(context, categoryUuid);
        var categoryResponse = mapper.Map<CategoryResponse>(categoryEntity);

        return categoryResponse;
    }

    public async Task<PaginationResponse<CategoryResponse>> Search(CategorySearchRequest request)
    {
        request.ValidateCategorySearchArguments();

        var categoryEntities = await CategoryHelperMethods.SearchCategoryEntities(context, request);
        var categoriesResponse = mapper.Map<PaginationResponse<CategoryResponse>>(categoryEntities);

        return categoriesResponse;
    }

    public async Task<CategoryResponse> Create(CategoryCreateRequest request)
    {
        request.ValidateCategoryCreateArguments();

        var categoryEntity = mapper.Map<Category>(request);
        await categoryEntity.CreateCategory(context);

        return mapper.Map<CategoryResponse>(categoryEntity);
    }
}
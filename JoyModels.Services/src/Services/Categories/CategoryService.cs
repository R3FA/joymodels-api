using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;
using JoyModels.Services.Services.Categories.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.Categories;

namespace JoyModels.Services.Services.Categories;

public class CategoryService(JoyModelsDbContext context, IMapper mapper, UserAuthValidation userAuthValidation)
    : ICategoryService
{
    public async Task<CategoryResponse> GetByUuid(Guid categoryUuid)
    {
        var categoryEntity = await CategoryHelperMethods.GetCategoryEntity(context, categoryUuid);
        return mapper.Map<CategoryResponse>(categoryEntity);
    }

    public async Task<PaginationResponse<CategoryResponse>> Search(CategorySearchRequest request)
    {
        request.ValidateCategorySearchArguments();

        var categoryEntities = await CategoryHelperMethods.SearchCategoryEntities(context, request);

        return mapper.Map<PaginationResponse<CategoryResponse>>(categoryEntities);
    }

    public async Task<CategoryResponse> Create(CategoryCreateRequest request)
    {
        request.ValidateCategoryCreateArguments();

        var categoryEntity = mapper.Map<Category>(request);
        await categoryEntity.CreateCategory(context);

        return mapper.Map<CategoryResponse>(categoryEntity);
    }

    public async Task<CategoryResponse> Patch(Guid categoryUuid, CategoryPatchRequest request)
    {
        userAuthValidation.ValidateRequestUuids(categoryUuid, request.Uuid);
        request.ValidateCategoryPatchArguments();

        await request.PatchCategory(context);

        return mapper.Map<CategoryResponse>(request);
    }

    public async Task Delete(Guid categoryUuid)
    {
        await CategoryHelperMethods.DeleteCategory(context, categoryUuid);
    }
}
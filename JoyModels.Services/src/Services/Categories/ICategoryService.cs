using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;

namespace JoyModels.Services.Services.Categories;

public interface ICategoryService
{
    Task<CategoryResponse> GetByUuid(Guid categoryUuid);

    Task<PaginationResponse<CategoryResponse>> Search(CategorySearchRequest request);
    Task<CategoryResponse> Create(CategoryCreateRequest request);
    Task<CategoryResponse> Patch(CategoryPatchRequest request);
    Task Delete(Guid categoryUuid);
}
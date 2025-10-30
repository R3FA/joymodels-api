using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;

namespace JoyModels.Services.Services.Categories;

public interface ICategoryService
{
    Task<CategoryResponse> GetByUuid(Guid categoryUuid);

    Task<PaginationResponse<CategoryResponse>> Search(CategorySearchRequest request);
    Task<CategoryResponse> Create(CategoryCreateRequest request);
}
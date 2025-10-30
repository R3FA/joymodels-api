using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Categories;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Categories;

namespace JoyModels.Models.AutoMapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResponse>();
        CreateMap<CategoryCreateRequest, Category>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid());
        CreateMap<CategoryPatchRequest, CategoryResponse>();
    }
}
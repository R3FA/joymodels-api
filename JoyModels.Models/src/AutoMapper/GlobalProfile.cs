using AutoMapper;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.Pagination;

namespace JoyModels.Models.AutoMapper;

public class GlobalProfile : Profile
{
    public GlobalProfile()
    {
        CreateMap(typeof(PaginationBase<>), typeof(PaginationResponse<>));
    }
}
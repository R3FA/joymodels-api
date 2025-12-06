using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ShoppingCart;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ShoppingCart;

namespace JoyModels.Models.AutoMapper;

public class ShoppingCartProfile : Profile
{
    public ShoppingCartProfile()
    {
        CreateMap<ShoppingCart, ShoppingCartResponse>()
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.ModelUu));

        CreateMap<ShoppingCartCreateRequest, ShoppingCart>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);
    }
}
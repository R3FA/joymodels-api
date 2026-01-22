using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Order;

namespace JoyModels.Models.AutoMapper;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model));
    }
}
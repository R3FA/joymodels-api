using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelAvailability;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;

namespace JoyModels.Models.AutoMapper;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<Model, ModelResponse>()
            .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.UserUu))
            .ForMember(dest => dest.ModelAvailability,
                opt => opt.MapFrom(src => src.ModelAvailabilityUu))
            .ForMember(dest => dest.ModelCategories,
                opt => opt.MapFrom(src => src.ModelCategories.Select(x => x.CategoryUu)));
        CreateMap<ModelAvailabilityResponse, ModelAvailability>();
        CreateMap<ModelCreateRequest, Model>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);
    }
}
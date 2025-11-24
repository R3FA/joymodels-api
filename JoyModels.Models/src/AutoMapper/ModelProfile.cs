using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelPicture;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

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
        CreateMap<ModelCreateRequest, Model>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);
        CreateMap<ModelPicture, ModelPictureResponse>();
        CreateMap<UserModelLike, UserModelLikesSearchResponse>()
            .ForMember(dest => dest.ModelResponse,
                opt => opt.MapFrom(src => src.ModelUu));
    }
}
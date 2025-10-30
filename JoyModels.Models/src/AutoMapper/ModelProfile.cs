using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;

namespace JoyModels.Models.AutoMapper;

public class ModelProfile : Profile
{
    public ModelProfile()
    {
        CreateMap<Model, ModelResponse>()
            .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.UserUu));
    }
}
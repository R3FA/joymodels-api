using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.AutoMapper;

public class UserModelLikeProfile : Profile
{
    public UserModelLikeProfile()
    {
        CreateMap<UserModelLike, UserModelLikesSearchResponse>()
            .ForMember(dest => dest.ModelResponse,
                opt => opt.MapFrom(src => src.ModelUu));
    }
}
using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserFollowers;

namespace JoyModels.Models.AutoMapper;

public class UserFollowersProfile : Profile
{
    public UserFollowersProfile()
    {
        CreateMap<UserFollower, UserFollowingResponse>()
            .ForMember(dest => dest.TargetUser,
                opt => opt.MapFrom(src => src.UserTargetUu));
        CreateMap<UserFollower, UserFollowerResponse>()
            .ForMember(dest => dest.OriginUser,
                opt => opt.MapFrom(src => src.UserOriginUu));
    }
}
using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;

namespace JoyModels.Models.AutoMapper;

public class UsersProfile : Profile
{
    public UsersProfile()
    {
        CreateMap<User, UsersResponse>()
            .ForMember(dest => dest.UserRole,
                opt => opt.MapFrom(src => src.UserRoleUu))
            .ForMember(dest => dest.UserModelLikes,
                opt => opt.MapFrom(src => src.UserModelLikes.Count))
            .ForMember(dest => dest.UserFollowers,
                opt => opt.MapFrom(src => src.UserFollowerUserTargetUus.Count))
            .ForMember(dest => dest.UserFollowing,
                opt => opt.MapFrom(src => src.UserFollowerUserTargetUus.Count));
        ;
    }
}
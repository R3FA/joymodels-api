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
                opt => opt.MapFrom(src => src.UserRoleUu));
    }
}
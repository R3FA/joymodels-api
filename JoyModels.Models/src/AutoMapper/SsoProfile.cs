using AutoMapper;
using JoyModels.Models.DataTransferObjects.CustomReturnTypes;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.UserRole;
using JoyModels.Models.Pagination;
using JoyModels.Models.src.Database.Entities;

namespace JoyModels.Models.AutoMapper;

public class SsoProfile : Profile
{
    public SsoProfile()
    {
        CreateMap<PendingUser, SsoReturn>()
            .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.UserUu));

        CreateMap<User, SsoUserGet>()
            .ForMember(dest => dest.UserRole,
                opt => opt.MapFrom(src => src.UserRoleUu));

        CreateMap<UserRole, UserRoleGet>();

        CreateMap<SsoUserCreate, User>();

        CreateMap<User, User>()
            .AfterMap((_, dest, context) =>
            {
                if (context.TryGetItems(out var items) &&
                    items.TryGetValue("UserRole", out var userRoleObject) &&
                    userRoleObject is UserRole userRole)
                {
                    dest.UserRoleUu = userRole;
                }
            });

        CreateMap<User, PendingUser>()
            .ForMember(dest => dest.UserUuid, opt => opt.MapFrom(src => src.Uuid));

        CreateMap<Guid, PendingUser>()
            .ForMember(dest => dest.UserUuid, opt => opt.MapFrom(src => src));

        CreateMap<SsoVerify, SsoGetByUuid>();

        CreateMap(typeof(PaginationBase<>), typeof(PaginationResponse<>));
    }
}
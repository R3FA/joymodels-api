using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Models.DataTransferObjects.UserRole;
using JoyModels.Models.Pagination;

namespace JoyModels.Models.AutoMapper;

public class SsoProfile : Profile
{
    public SsoProfile()
    {
        CreateMap<PendingUser, SsoUserResponse>()
            .ForMember(dest => dest.UserRole,
                opt => opt.MapFrom(src => src.UserUu.UserRoleUu));
        CreateMap<User, SsoUserResponse>()
            .ForMember(dest => dest.UserRole,
                opt => opt.MapFrom(src => src.UserRoleUu))
            .AfterMap((_, dest, context) =>
            {
                if (context.TryGetItems(out var items) &&
                    items.TryGetValue("UserAccessToken", out var userAccessTokenObject) &&
                    userAccessTokenObject is string userAccessToken)
                {
                    dest.UserAccessToken = userAccessToken;
                }
            });
        CreateMap<UserRole, UserRoleGet>();
        CreateMap<SsoUserCreateRequest, User>();
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
        CreateMap(typeof(PaginationBase<>), typeof(PaginationResponse<>));
        CreateMap<SsoAccessTokenChangeRequest, SsoLogoutRequest>();
        CreateMap<SsoVerifyRequest, SsoAccessTokenChangeRequest>();
    }
}
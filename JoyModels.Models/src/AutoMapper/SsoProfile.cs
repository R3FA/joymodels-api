using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Email;
using JoyModels.Models.DataTransferObjects.RequestTypes.Sso;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Sso;
using JoyModels.Models.DataTransferObjects.UserRole;

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
        CreateMap<SsoAccessTokenChangeRequest, SsoLogoutRequest>();
        CreateMap<SsoVerifyRequest, SsoAccessTokenChangeRequest>();
        CreateMap<(PendingUser, User), EmailSendUserDetailsRequest>()
            .ForMember(x => x.Email, o => o.MapFrom(z => z.Item2.Email))
            .ForMember(x => x.OtpCode, o => o.MapFrom(z => z.Item1.OtpCode))
            .ForMember(x => x.OtpExpirationDate, o => o.MapFrom(z => z.Item1.OtpExpirationDate));
    }
}
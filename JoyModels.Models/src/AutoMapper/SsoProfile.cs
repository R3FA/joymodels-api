using AutoMapper;
using JoyModels.Models.DataTransferObjects.Sso;
using JoyModels.Models.DataTransferObjects.User;
using JoyModels.Models.DataTransferObjects.UserRole;
using JoyModels.Models.src.Database.Entities;

namespace JoyModels.Models.AutoMapper;

public class SsoProfile : Profile
{
    public SsoProfile()
    {
        CreateMap<PendingUser, SsoGet>()
            .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.UserUu));

        CreateMap<User, UserGet>()
            .ForMember(dest => dest.UserRole,
                opt => opt.MapFrom(src => src.UserRoleUu));

        CreateMap<UserRole, UserRoleGet>();

        CreateMap<UserCreate, User>()
            .ForMember(dest => dest.Uuid, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

        CreateMap<User, PendingUser>()
            .ForMember(dest => dest.Uuid, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.UserUuid, opt => opt.MapFrom(src => src.Uuid))
            .ForMember(dest => dest.OtpCreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.OtpExpirationDate, opt => opt.MapFrom(_ => DateTime.Now.AddMinutes(60)));
    }
}
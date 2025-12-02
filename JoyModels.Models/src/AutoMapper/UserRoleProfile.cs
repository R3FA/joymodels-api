using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.UserRole;
using JoyModels.Models.DataTransferObjects.ResponseTypes.UserRole;

namespace JoyModels.Models.AutoMapper;

public class UserRoleProfile : Profile
{
    public UserRoleProfile()
    {
        CreateMap<UserRole, UserRoleResponse>();

        CreateMap<UserRoleCreateRequest, UserRole>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid());
    }
}
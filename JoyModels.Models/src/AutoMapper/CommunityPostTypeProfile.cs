using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostType;

namespace JoyModels.Models.AutoMapper;

public class CommunityPostTypeProfile : Profile
{
    public CommunityPostTypeProfile()
    {
        CreateMap<CommunityPostType, CommunityPostTypeResponse>();

        CreateMap<CommunityPostTypeCreateRequest, CommunityPostType>()
            .ForMember(dest => dest.CommunityPostName, opt => opt.MapFrom(src => src.PostTypeName))
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid());
    }
}
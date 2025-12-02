using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostReviewType;

namespace JoyModels.Models.AutoMapper;

public class CommunityPostReviewTypeProfile : Profile
{
    public CommunityPostReviewTypeProfile()
    {
        CreateMap<CommunityPostReviewType, CommunityPostReviewTypeResponse>();

        CreateMap<CommunityPostReviewTypeCreateRequest, CommunityPostReviewType>()
            .ForMember(dest => dest.ReviewName, opt => opt.MapFrom(src => src.CommunityPostReviewTypeName))
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid());
    }
}
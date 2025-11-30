using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;

namespace JoyModels.Models.AutoMapper;

public class CommunityPostProfile : Profile
{
    public CommunityPostProfile()
    {
        CreateMap<CommunityPostType, CommunityPostTypeResponse>();

        CreateMap<CommunityPostReviewType, CommunityPostReviewTypeResponse>();

        CreateMap<CommunityPostUserReview, CommunityPostUserReviewResponse>()
            .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.UserUu))
            .ForMember(dest => dest.ReviewType,
                opt => opt.MapFrom(src => src.ReviewTypeUu));

        CreateMap<CommunityPostPicture, CommunityPostPictureResponse>();

        CreateMap<CommunityPost, CommunityPostResponse>()
            .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.UserUu))
            .ForMember(dest => dest.CommunityPostType,
                opt => opt.MapFrom(src => src.PostTypeUu))
            .ForMember(dest => dest.PictureLocations,
                opt => opt.MapFrom(src => src.CommunityPostPictures));

        CreateMap<CommunityPostCreateRequest, CommunityPost>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);
    }
}
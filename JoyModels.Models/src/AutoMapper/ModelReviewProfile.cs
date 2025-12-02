using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviewsType;

namespace JoyModels.Models.AutoMapper;

public class ModelReviewProfile : Profile
{
    public ModelReviewProfile()
    {
        CreateMap<ModelReview, ModelReviewResponse>()
            .ForMember(x => x.ModelReviewText, o => o.MapFrom(z => z.ReviewText))
            .ForMember(x => x.ModelResponse, o => o.MapFrom(z => z.ModelUu))
            .ForMember(x => x.UsersResponse, o => o.MapFrom(z => z.UserUu))
            .ForMember(x => x.ModelReviewTypeResponse, o => o.MapFrom(z => z.ReviewTypeUu));
        CreateMap<ModelReviewCreateRequest, ModelReview>()
            .ForMember(x => x.ReviewTypeUuid, o => o.MapFrom(z => z.ModelReviewTypeUuid))
            .ForMember(x => x.ReviewText, o => o.MapFrom(z => z.ModelReviewText))
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);
    }
}
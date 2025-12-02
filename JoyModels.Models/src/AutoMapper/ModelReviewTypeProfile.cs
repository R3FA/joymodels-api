using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviewsType;

namespace JoyModels.Models.AutoMapper;

public class ModelReviewTypeProfile : Profile
{
    public ModelReviewTypeProfile()
    {
        CreateMap<ModelReviewType, ModelReviewTypeResponse>()
            .ForMember(x => x.ReviewType, o => o.MapFrom(z => z.ReviewName));

        CreateMap<ModelReviewTypeCreateRequest, ModelReviewType>()
            .ForMember(x => x.ReviewName, o => o.MapFrom(z => z.ModelReviewTypeName))
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid());
    }
}
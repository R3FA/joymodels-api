using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;

namespace JoyModels.Models.AutoMapper;

public class ModelFaqSectionProfile : Profile
{
    public ModelFaqSectionProfile()
    {
        CreateMap<ModelFaqSection, ModelFaqSectionResponse>()
            .ForMember(dest => dest.Model,
                opt => opt.MapFrom(src => src.ModelUu))
            .ForMember(dest => dest.User,
                opt => opt.MapFrom(src => src.UserUu));

        CreateMap<ModelFaqSectionCreateRequest, ModelFaqSection>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.ParentMessageUuid = null)
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);

        CreateMap<ModelFaqSectionCreateAnswerRequest, ModelFaqSection>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);
    }
}
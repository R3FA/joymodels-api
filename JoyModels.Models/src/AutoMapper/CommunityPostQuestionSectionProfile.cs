using AutoMapper;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;

namespace JoyModels.Models.AutoMapper;

public class CommunityPostQuestionSectionProfile : Profile
{
    public CommunityPostQuestionSectionProfile()
    {
        CreateMap<CommunityPostQuestionSection, CommunityPostQuestionSectionResponse>()
            .ForMember(dest => dest.CommunityPost, opt => opt.MapFrom(src => src.CommunityPostUu))
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserUu));

        CreateMap<CommunityPostQuestionSection, CommunityPostQuestionSectionParent>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserUu));

        CreateMap<CommunityPostQuestionSection, CommunityPostQuestionSectionReply>()
            .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserUu));

        CreateMap<CommunityPostQuestionSectionCreateRequest, CommunityPostQuestionSection>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.ParentMessageUuid = null)
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);

        CreateMap<CommunityPostQuestionSectionCreateAnswerRequest, CommunityPostQuestionSection>()
            .AfterMap((_, dest) => dest.Uuid = Guid.NewGuid())
            .AfterMap((_, dest) => dest.CreatedAt = DateTime.Now);
    }
}
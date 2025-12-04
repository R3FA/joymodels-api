using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;
using JoyModels.Services.Services.CommunityPostQuestionSection.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.CommunityPostQuestionSection;

public class CommunityPostQuestionSectionService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation)
    : ICommunityPostQuestionSectionService
{
    public async Task<CommunityPostQuestionSectionResponse> GetByUuid(Guid communityPostQuestionSectionUuid)
    {
        var communityPostQuestionSectionEntity =
            await CommunityPostQuestionSectionHelperMethods.GetCommunityPostQuestionSectionEntity(context,
                communityPostQuestionSectionUuid);

        return mapper.Map<CommunityPostQuestionSectionResponse>(communityPostQuestionSectionEntity);
    }

    public async Task<CommunityPostQuestionSectionResponse> Create(CommunityPostQuestionSectionCreateRequest request)
    {
        request.ValidateCommunityPostQuestionSectionCreateArguments();

        var communityPostQuestionSectionEntity =
            mapper.Map<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>(request);
        communityPostQuestionSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await communityPostQuestionSectionEntity.CreateCommunityPostQuestionSectionEntity(context);

        return await GetByUuid(communityPostQuestionSectionEntity.Uuid);
    }

    public async Task<CommunityPostQuestionSectionResponse> CreateAnswer(
        CommunityPostQuestionSectionCreateAnswerRequest request)
    {
        await request.ValidateCommunityPostSectionCreateAnswerArguments(context);

        var modelFaqSectionEntity =
            mapper.Map<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>(request);
        modelFaqSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await modelFaqSectionEntity.CreateCommunityPostQuestionSectionEntity(context);

        return await GetByUuid(modelFaqSectionEntity.Uuid);
    }

    public async Task<CommunityPostQuestionSectionResponse> Patch(CommunityPostQuestionSectionPatchRequest request)
    {
        request.ValidateCommunityPostQuestionSectionPatchArguments();

        await request.PatchCommunityPostQuestionSectionEntity(context, userAuthValidation);

        return await GetByUuid(request.CommunityPostQuestionSectionUuid);
    }

    public async Task Delete(Guid communityPostQuestionSectionUuid)
    {
        await CommunityPostQuestionSectionHelperMethods.DeleteCommunityPostQuestionSectionEntity(context,
            userAuthValidation, communityPostQuestionSectionUuid);
    }
}
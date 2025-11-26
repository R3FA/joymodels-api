using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;
using JoyModels.Services.Services.ModelFaqSection.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.ModelFaqSection;

public class ModelFaqSectionService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation)
    : IModelFaqSectionService
{
    public async Task<ModelFaqSectionResponse> GetByUuid(Guid modelFaqSectionUuid)
    {
        var modelFaqSectionEntity =
            await ModelFaqSectionHelperMethods.GetModelFaqSectionEntity(context, modelFaqSectionUuid);
        return mapper.Map<ModelFaqSectionResponse>(modelFaqSectionEntity);
    }

    public async Task<ModelFaqSectionResponse> Create(ModelFaqSectionCreateRequest request)
    {
        request.ValidateModelFaqSectionCreateArguments();

        var modelFaqSectionEntity = mapper.Map<JoyModels.Models.Database.Entities.ModelFaqSection>(request);
        modelFaqSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await modelFaqSectionEntity.CreateModelFaqSectionEntity(context);

        return await GetByUuid(modelFaqSectionEntity.Uuid);
    }

    public async Task<ModelFaqSectionResponse> CreateAnswer(ModelFaqSectionCreateAnswerRequest request)
    {
        await request.ValidateModelFaqSectionCreateAnswerArguments(context);

        var modelFaqSectionEntity = mapper.Map<JoyModels.Models.Database.Entities.ModelFaqSection>(request);
        modelFaqSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await modelFaqSectionEntity.CreateModelFaqSectionEntity(context);

        return await GetByUuid(modelFaqSectionEntity.Uuid);
    }

    public async Task Delete(Guid modelFaqSectionUuid)
    {
        throw new NotImplementedException();
    }
}
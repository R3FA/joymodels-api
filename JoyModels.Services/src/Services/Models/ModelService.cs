using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Models.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.Models;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.Models;

public class ModelService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    ImageSettingsDetails imageSettingsDetails,
    ModelSettingsDetails modelSettingsDetails)
    : IModelService
{
    private async Task<ModelResponse> GetByUuidWithAllAvailabilities(Guid modelUuid)
    {
        var modelEntity = await ModelHelperMethods.GetModelEntityWithAllAvailabilities(context, modelUuid);
        return mapper.Map<ModelResponse>(modelEntity);
    }

    public async Task<ModelResponse> GetByUuid(Guid modelUuid)
    {
        var modelEntity =
            await ModelHelperMethods.GetModelEntity(context, modelUuid);
        return mapper.Map<ModelResponse>(modelEntity);
    }

    public async Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request)
    {
        request.ValidateModelSearchArguments();

        var modelEntities = await ModelHelperMethods.SearchModelEntities(context, request, userAuthValidation);

        return mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);
    }

    public async Task<ModelResponse> Create(ModelCreateRequest request)
    {
        request.ValidateModelCreateArguments();

        var modelEntity = mapper.Map<Model>(request);
        modelEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        var modelPicturePaths = await request.Pictures.SaveModelPictures(imageSettingsDetails, modelEntity.Uuid);
        modelEntity.LocationPath =
            await request.Model.SaveModel(modelSettingsDetails, modelEntity.Uuid, modelPicturePaths);

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await modelEntity.CreateModelEntity(context);
            await modelEntity.CreateModelCategories(context, request);
            await ModelHelperMethods.CreateModelPictures(modelEntity, context, modelPicturePaths);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            ModelHelperMethods.DeleteModelPictureUuidFolderOnException(modelPicturePaths[0]);
            ModelHelperMethods.DeleteModelUuidFolderOnException(modelEntity.LocationPath);

            throw new TransactionException(ex.InnerException!.Message);
        }

        return await GetByUuidWithAllAvailabilities(modelEntity.Uuid);
    }

    public async Task<ModelResponse> Patch(Guid modelUuid, ModelPatchRequest request)
    {
        userAuthValidation.ValidateRequestUuids(modelUuid, request.Uuid);
        var modelResponse = await GetByUuidWithAllAvailabilities(modelUuid);
        userAuthValidation.ValidateUserAuthRequest(modelResponse.UserUuid);
        request.ValidateModelPatchArguments();
        await request.ValidateModelPatchArgumentsDuplicatedFields(context);

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await request.PatchModelEntity(modelResponse, imageSettingsDetails, context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.Message);
        }

        return await GetByUuidWithAllAvailabilities(modelUuid);
    }

    public async Task Delete(Guid modelUuid)
    {
        var modelEntity = await GetByUuidWithAllAvailabilities(modelUuid);

        if (userAuthValidation.GetUserClaimRole() != nameof(UserRoleEnum.Admin)
            && userAuthValidation.GetUserClaimRole() != nameof(UserRoleEnum.Root))
            userAuthValidation.ValidateUserAuthRequest(modelEntity.UserUuid);

        await ModelHelperMethods.DeleteModel(context, modelUuid);

        ModelHelperMethods.DeleteModelPictureUuidFolderOnException(modelEntity.ModelPictures[0].PictureLocation);
        ModelHelperMethods.DeleteModelUuidFolderOnException(modelEntity.LocationPath);
    }
}
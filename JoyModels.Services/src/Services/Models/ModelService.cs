using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Core;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.Models.HelperMethods;
using JoyModels.Services.Services.Recommender;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Models;

public class ModelService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    ModelImageSettingsDetails modelImageSettingsDetails,
    ModelSettingsDetails modelSettingsDetails,
    IMessageProducer messageProducer,
    IRecommenderService recommenderService)
    : IModelService
{
    private async Task<ModelResponse> GetByUuidWithAllAvailabilities(Guid modelUuid)
    {
        var modelEntity = await ModelHelperMethods.GetModelEntityWithAllAvailabilities(context, modelUuid);
        return mapper.Map<ModelResponse>(modelEntity);
    }

    public async Task<ModelResponse> GetByUuid(ModelGetByUuidRequest request)
    {
        var modelEntity =
            await ModelHelperMethods.GetModelEntity(context, request, userAuthValidation);
        return mapper.Map<ModelResponse>(modelEntity);
    }

    public async Task<PictureResponse> GetModelPictures(Guid modelUuid, string modelPictureFileName)
    {
        await GetByUuid(new ModelGetByUuidRequest { ModelUuid = modelUuid });

        var fileName = Uri.UnescapeDataString(modelPictureFileName);

        var realPath = Path.Combine(modelImageSettingsDetails.SavePath, "models", modelUuid.ToString(), fileName);

        if (!File.Exists(realPath))
            throw new KeyNotFoundException("Model picture doesn't exist");

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(realPath, out var contentType))
            contentType = "application/octet-stream";

        var fileBytes = await File.ReadAllBytesAsync(realPath);

        return new PictureResponse
        {
            FileBytes = fileBytes,
            ContentType = contentType,
        };
    }

    public async Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request)
    {
        var modelEntities = await ModelHelperMethods.SearchModelEntities(context, request, userAuthValidation);

        return mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);
    }

    public async Task<PaginationResponse<ModelResponse>> AdminSearch(ModelAdminSearchRequest request)
    {
        var modelEntities = await ModelHelperMethods.SearchAdminModelEntities(context, request);

        return mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);
    }

    public async Task<PaginationResponse<ModelResponse>> BestSelling(ModelBestSellingRequest request)
    {
        var modelEntities = await ModelHelperMethods.SearchBestSellingModelEntities(context, request);

        return mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);
    }

    public async Task<PaginationResponse<ModelResponse>> Recommended(ModelRecommendedRequest request)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();
        var fetchCount = request.PageNumber * request.PageSize;

        var recommendedModelUuids = await recommenderService.GetRecommendations(userUuid, fetchCount);

        var modelEntities =
            await ModelHelperMethods.GetRecommendedModelEntities(context, recommendedModelUuids, request);

        return mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);
    }

    public async Task<bool> IsModelLiked(Guid modelUuid)
    {
        return await context.UserModelLikes
            .AnyAsync(x =>
                x.UserUuid == userAuthValidation.GetUserClaimUuid()
                && x.ModelUuid == modelUuid);
    }

    public async Task<ModelResponse> Create(ModelCreateRequest request)
    {
        request.ValidateModelCreateArguments();

        var modelEntity = mapper.Map<Model>(request);
        modelEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        var modelPicturePaths = await request.Pictures.SaveModelPictures(modelImageSettingsDetails, modelEntity.Uuid);
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

    public async Task ModelLike(Guid modelUuid)
    {
        await ModelValidation.ValidateModelLikeEndpoint(context, modelUuid, userAuthValidation);

        var userModelLikeEntity = ModelHelperMethods.CreateUserModelLikeObject(modelUuid, userAuthValidation);
        await userModelLikeEntity.CreateUserModelLikeEntity(context);

        var likerUuid = userAuthValidation.GetUserClaimUuid();
        var model = await context.Models.FirstAsync(x => x.Uuid == modelUuid);

        if (model.UserUuid != likerUuid)
        {
            var liker = await context.Users.FirstAsync(x => x.Uuid == likerUuid);

            var notification = new CreateNotificationRequest
            {
                ActorUuid = likerUuid,
                TargetUserUuid = model.UserUuid,
                NotificationType = nameof(NotificationType.ModelLiked),
                Title = "Model Liked",
                Message = $"{liker.NickName} liked your model '{model.Name}'.",
                RelatedEntityUuid = modelUuid,
                RelatedEntityType = "Model"
            };
            await messageProducer.SendMessage("create_notification", notification);
        }
    }

    public async Task<ModelResponse> Patch(ModelPatchRequest request)
    {
        var modelResponse = await GetByUuidWithAllAvailabilities(request.Uuid);
        request.ValidateModelPatchArguments();
        await request.ValidateModelPatchArgumentsDuplicatedFields(context);

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await request.PatchModelEntity(modelResponse, modelImageSettingsDetails, context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.Message);
        }

        return await GetByUuidWithAllAvailabilities(request.Uuid);
    }

    public async Task ModelUnlike(Guid modelUuid)
    {
        await ModelValidation.ValidateModelUnlikeEndpoint(context, modelUuid, userAuthValidation);

        await ModelHelperMethods.DeleteUserModelLikeEntity(context, modelUuid, userAuthValidation);
    }

    public async Task Delete(Guid modelUuid)
    {
        var modelEntity = await GetByUuidWithAllAvailabilities(modelUuid);

        await ModelHelperMethods.DeleteModel(context, modelUuid, userAuthValidation);

        ModelHelperMethods.DeleteModelPictureUuidFolderOnException(modelEntity.ModelPictures[0].PictureLocation);
        ModelHelperMethods.DeleteModelUuidFolderOnException(modelEntity.LocationPath);
    }
}
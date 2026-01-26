using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.ModelReviews.HelperMethods;
using JoyModels.Services.Services.Models;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.ModelReviews;

public class ModelReviewService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    IModelService modelService,
    IMessageProducer messageProducer)
    : IModelReviewService
{
    public async Task<ModelReviewResponse> GetByUuid(Guid modelReviewUuid)
    {
        var modelReviewEntity = await ModelReviewHelperMethods.GetModelReviewEntity(context, modelReviewUuid);

        return mapper.Map<ModelReviewResponse>(modelReviewEntity);
    }

    public async Task<PaginationResponse<ModelReviewResponse>> Search(ModelReviewSearchRequest request)
    {
        var modelReviewEntities = await ModelReviewHelperMethods.SearchModelReviewEntities(context, request);

        return mapper.Map<PaginationResponse<ModelReviewResponse>>(modelReviewEntities);
    }

    public async Task<bool> HasUserReviewed(Guid modelUuid)
    {
        return await context.ModelReviews.AnyAsync(x =>
            x.ModelUuid == modelUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid());
    }

    public async Task<ModelCalculatedReviewsResponse> CalculateReviews(Guid modelUuid)
    {
        return await ModelReviewHelperMethods.CalculateReviews(context, modelUuid);
    }

    public async Task<ModelReviewResponse> Create(ModelReviewCreateRequest request)
    {
        var modelResponse = await modelService.GetByUuid(new ModelGetByUuidRequest
            { ArePrivateModelsSearched = false, ModelUuid = request.ModelUuid });

        request.ValidateModelReviewCreateArguments(modelResponse, userAuthValidation.GetUserClaimUuid());
        await ModelReviewValidation.ValidateDuplicatedModelReviews(context, modelResponse.Uuid,
            userAuthValidation.GetUserClaimUuid());
        await ModelReviewValidation.ValidateModelOwnership(context, modelResponse.Uuid,
            userAuthValidation.GetUserClaimUuid());

        var modelReviewEntity = mapper.Map<ModelReview>(request);
        modelReviewEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await modelReviewEntity.CreateModelReviewEntity(context);

        var reviewerUuid = userAuthValidation.GetUserClaimUuid();
        if (modelResponse.UserUuid != reviewerUuid)
        {
            var reviewer = await context.Users.FirstAsync(x => x.Uuid == reviewerUuid);

            var notification = new CreateNotificationRequest
            {
                ActorUuid = reviewerUuid,
                TargetUserUuid = modelResponse.UserUuid,
                NotificationType = nameof(NotificationType.NewModelReview),
                Title = "New Review",
                Message = $"{reviewer.NickName} reviewed your model '{modelResponse.Name}'.",
                RelatedEntityUuid = modelReviewEntity.Uuid,
                RelatedEntityType = "ModelReview"
            };
            await messageProducer.SendMessage("create_notification", notification);
        }

        return await GetByUuid(modelReviewEntity.Uuid);
    }

    public async Task<ModelReviewResponse> Patch(ModelReviewPatchRequest request)
    {
        var modelReviewResponse = await GetByUuid(request.ModelReviewUuid);
        userAuthValidation.ValidateUserAuthRequest(modelReviewResponse.UsersResponse.Uuid);
        request.ValidateModelReviewPatchArguments();

        await request.PatchModelEntity(context, userAuthValidation);

        return await GetByUuid(request.ModelReviewUuid);
    }

    public async Task Delete(Guid modelReviewUuid)
    {
        await ModelReviewHelperMethods.DeleteModelReview(context, modelReviewUuid, userAuthValidation);
    }
}
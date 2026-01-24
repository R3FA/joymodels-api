using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelFaqSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.ModelFaqSection.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.ModelFaqSection;

public class ModelFaqSectionService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    IMessageProducer messageProducer)
    : IModelFaqSectionService
{
    public async Task<ModelFaqSectionResponse> GetByUuid(Guid modelFaqSectionUuid)
    {
        var modelFaqSectionEntity =
            await ModelFaqSectionHelperMethods.GetModelFaqSectionEntity(context, modelFaqSectionUuid);
        return mapper.Map<ModelFaqSectionResponse>(modelFaqSectionEntity);
    }

    public async Task<PaginationResponse<ModelFaqSectionResponse>> Search(ModelFaqSectionSearchRequest request)
    {
        var modelFaqEntities = await ModelFaqSectionHelperMethods.SearchModelFaqEntities(context, request);

        return mapper.Map<PaginationResponse<ModelFaqSectionResponse>>(modelFaqEntities);
    }

    public async Task<ModelFaqSectionResponse> Create(ModelFaqSectionCreateRequest request)
    {
        request.ValidateModelFaqSectionCreateArguments();

        var modelFaqSectionEntity = mapper.Map<JoyModels.Models.Database.Entities.ModelFaqSection>(request);
        modelFaqSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await modelFaqSectionEntity.CreateModelFaqSectionEntity(context);

        var askerUuid = userAuthValidation.GetUserClaimUuid();
        var model = await context.Models.FirstAsync(x => x.Uuid == request.ModelUuid);

        if (model.UserUuid != askerUuid)
        {
            var asker = await context.Users.FirstAsync(x => x.Uuid == askerUuid);

            var notification = new CreateNotificationRequest
            {
                ActorUuid = askerUuid,
                TargetUserUuid = model.UserUuid,
                NotificationType = nameof(NotificationType.NewComment),
                Title = "New Question",
                Message = $"{asker.NickName} asked a question on your model '{model.Name}'.",
                RelatedEntityUuid = modelFaqSectionEntity.Uuid,
                RelatedEntityType = "ModelFaqSection"
            };
            await messageProducer.SendMessage("create_notification", notification);
        }

        return await GetByUuid(modelFaqSectionEntity.Uuid);
    }

    public async Task<ModelFaqSectionResponse> CreateAnswer(ModelFaqSectionCreateAnswerRequest request)
    {
        await request.ValidateModelFaqSectionCreateAnswerArguments(context);

        var modelFaqSectionEntity = mapper.Map<JoyModels.Models.Database.Entities.ModelFaqSection>(request);
        modelFaqSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await modelFaqSectionEntity.CreateModelFaqSectionEntity(context);

        var replierUuid = userAuthValidation.GetUserClaimUuid();
        var parentMessage = await context.ModelFaqSections
            .Include(x => x.ModelUu)
            .FirstAsync(x => x.Uuid == request.ParentMessageUuid);

        if (parentMessage.UserUuid != replierUuid)
        {
            var replier = await context.Users.FirstAsync(x => x.Uuid == replierUuid);

            var notification = new CreateNotificationRequest
            {
                ActorUuid = replierUuid,
                TargetUserUuid = parentMessage.UserUuid,
                NotificationType = nameof(NotificationType.NewComment),
                Title = "New Reply",
                Message = $"{replier.NickName} replied to your comment on '{parentMessage.ModelUu.Name}'.",
                RelatedEntityUuid = modelFaqSectionEntity.Uuid,
                RelatedEntityType = "ModelFaqSection"
            };
            await messageProducer.SendMessage("create_notification", notification);
        }

        return await GetByUuid(modelFaqSectionEntity.Uuid);
    }

    public async Task<ModelFaqSectionResponse> Patch(ModelFaqSectionPatchRequest request)
    {
        request.ValidateModelFaqSectionPatchArguments();

        await request.PatchModelFaqSectionEntity(context, userAuthValidation);

        return await GetByUuid(request.ModelFaqSectionUuid);
    }

    public async Task Delete(ModelFaqSectionDeleteRequest request)
    {
        await request.DeleteModelFaqSectionEntity(context, userAuthValidation);
    }
}
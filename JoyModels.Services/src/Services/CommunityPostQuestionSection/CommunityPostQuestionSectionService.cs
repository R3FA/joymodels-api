using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.RequestTypes.Notification;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostQuestionSection;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.Enums;
using JoyModels.Services.Services.CommunityPostQuestionSection.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Utilities.RabbitMQ.MessageProducer;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.CommunityPostQuestionSection;

public class CommunityPostQuestionSectionService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    IMessageProducer messageProducer)
    : ICommunityPostQuestionSectionService
{
    public async Task<CommunityPostQuestionSectionResponse> GetByUuid(Guid communityPostQuestionSectionUuid)
    {
        var communityPostQuestionSectionEntity =
            await CommunityPostQuestionSectionHelperMethods.GetCommunityPostQuestionSectionEntity(context,
                communityPostQuestionSectionUuid);

        return mapper.Map<CommunityPostQuestionSectionResponse>(communityPostQuestionSectionEntity);
    }

    public async Task<PaginationResponse<CommunityPostQuestionSectionResponse>> Search(
        CommunityPostQuestionSectionSearchRequest request)
    {
        var communityPostQuestionSectionEntities =
            await CommunityPostQuestionSectionHelperMethods.SearchCommunityPostEntities(context, request);

        return mapper.Map<PaginationResponse<CommunityPostQuestionSectionResponse>>(
            communityPostQuestionSectionEntities);
    }

    public async Task<CommunityPostQuestionSectionResponse> Create(CommunityPostQuestionSectionCreateRequest request)
    {
        request.ValidateCommunityPostQuestionSectionCreateArguments();

        var communityPostQuestionSectionEntity =
            mapper.Map<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>(request);
        communityPostQuestionSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await communityPostQuestionSectionEntity.CreateCommunityPostQuestionSectionEntity(context);

        var commenterUuid = userAuthValidation.GetUserClaimUuid();
        var post = await context.CommunityPosts.FirstAsync(x => x.Uuid == request.CommunityPostUuid);

        if (post.UserUuid != commenterUuid)
        {
            var commenter = await context.Users.FirstAsync(x => x.Uuid == commenterUuid);

            var notification = new CreateNotificationRequest
            {
                ActorUuid = commenterUuid,
                TargetUserUuid = post.UserUuid,
                NotificationType = nameof(NotificationType.NewComment),
                Title = "New Comment",
                Message = $"{commenter.NickName} commented on your post '{post.Title}'.",
                RelatedEntityUuid = communityPostQuestionSectionEntity.Uuid,
                RelatedEntityType = "CommunityPostQuestionSection"
            };
            await messageProducer.SendMessage("create_notification", notification);
        }

        return await GetByUuid(communityPostQuestionSectionEntity.Uuid);
    }

    public async Task<CommunityPostQuestionSectionResponse> CreateAnswer(
        CommunityPostQuestionSectionCreateAnswerRequest request)
    {
        await request.ValidateCommunityPostSectionCreateAnswerArguments(context);

        var communityPostQuestionSectionEntity =
            mapper.Map<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>(request);
        communityPostQuestionSectionEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await communityPostQuestionSectionEntity.CreateCommunityPostQuestionSectionEntity(context);

        var replierUuid = userAuthValidation.GetUserClaimUuid();
        var parentMessage = await context.CommunityPostQuestionSections
            .Include(x => x.CommunityPostUu)
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
                Message = $"{replier.NickName} replied to your comment on '{parentMessage.CommunityPostUu.Title}'.",
                RelatedEntityUuid = communityPostQuestionSectionEntity.Uuid,
                RelatedEntityType = "CommunityPostQuestionSection"
            };
            await messageProducer.SendMessage("create_notification", notification);
        }

        return await GetByUuid(communityPostQuestionSectionEntity.Uuid);
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
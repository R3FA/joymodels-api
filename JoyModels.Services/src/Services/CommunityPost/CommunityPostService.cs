using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Services.Services.CommunityPost.HelperMethods;
using JoyModels.Services.Services.Models.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.CommunityPost;

public class CommunityPostService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    ModelImageSettingsDetails modelImageSettingsDetails) : ICommunityPostService
{
    public async Task<CommunityPostResponse> GetByUuid(Guid communityPostUuid)
    {
        var communityPostEntity = await CommunityPostHelperMethods.GetCommunityPostEntity(context, communityPostUuid);

        return mapper.Map<CommunityPostResponse>(communityPostEntity);
    }

    public async Task<CommunityPostResponse> Create(CommunityPostCreateRequest request)
    {
        await request.ValidateCommunityPostCreateArguments(modelImageSettingsDetails);

        var communityPostEntity = mapper.Map<JoyModels.Models.Database.Entities.CommunityPost>(request);
        communityPostEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        List<CommunityPostPicture>? communityPostPictureEntities = null;

        if (request.Pictures != null)
        {
            var communityPostPicturePaths =
                await request.Pictures.SaveCommunityPostPictures(modelImageSettingsDetails, communityPostEntity.Uuid);
            communityPostPictureEntities =
                CommunityPostHelperMethods.CreateCommunityPostPictureEntities(communityPostEntity.Uuid,
                    communityPostPicturePaths);
        }

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await communityPostEntity.CreateCommunityPostEntity(context);
            await communityPostPictureEntities.CreateCommunityPostPictureEntities(context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            if (communityPostPictureEntities != null)
                ModelHelperMethods.DeleteModelPictureUuidFolderOnException(communityPostPictureEntities[0]
                    .PictureLocation[0].ToString());

            throw new TransactionException(ex.InnerException!.Message);
        }

        return await GetByUuid(communityPostEntity.Uuid);
    }

    public async Task CommunityPostLike(CommunityPostLikeRequest request)
    {
        await request.ValidateCommunityPostLikeArguments(context, userAuthValidation);

        await request.CreateCommunityPostUserReviewEntity(context, userAuthValidation);
    }
}
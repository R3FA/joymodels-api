using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPost;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
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

    public async Task<PaginationResponse<CommunityPostResponse>> Search(CommunityPostSearchRequest request)
    {
        request.ValidateCommunityPostSearchArguments();

        var communityPostEntities = await request.SearchCommunityPostEntities(context);

        return mapper.Map<PaginationResponse<CommunityPostResponse>>(communityPostEntities);
    }

    public async Task<PaginationResponse<CommunityPostUserReviewResponse>> SearchReviewedUsers(
        CommunityPostSearchReviewedUsersRequest request)
    {
        var communityPostUserReviewEntities = await request.SearchCommunityPostUserReviewEntities(context);

        return mapper.Map<PaginationResponse<CommunityPostUserReviewResponse>>(communityPostUserReviewEntities);
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
                CommunityPostHelperMethods.CreateCommunityPostPictureEntityInstances(communityPostEntity.Uuid,
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

    public async Task<CommunityPostResponse> Patch(CommunityPostPatchRequest request)
    {
        var communityPostResponse = await GetByUuid(request.CommunityPostUuid);
        userAuthValidation.ValidateUserAuthRequest(communityPostResponse.User.Uuid);

        await request.ValidateCommunityPostPatchArguments(context, communityPostResponse, modelImageSettingsDetails);

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await request.PatchCommunityPostEntity(modelImageSettingsDetails, context);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.Message);
        }

        return await GetByUuid(request.CommunityPostUuid);
    }

    public async Task CreateUserReview(CommunityPostUserReviewCreateRequest request)
    {
        await request.ValidateCommunityPostLikeArguments(context, userAuthValidation);

        await request.CreateCommunityPostUserReviewEntity(context, userAuthValidation);
    }

    public async Task DeleteUserReview(CommunityPostUserReviewDeleteRequest request)
    {
        await request.DeleteCommunityPostUserReview(context, userAuthValidation);
    }

    public async Task Delete(Guid communityPostUuid)
    {
        var communityPostEntity = await GetByUuid(communityPostUuid);

        await CommunityPostHelperMethods.DeleteCommunityPostEntity(context, communityPostUuid, userAuthValidation);

        if (communityPostEntity.PictureLocations.Count > 0)
            ModelHelperMethods.DeleteModelUuidFolderOnException(communityPostEntity.PictureLocations[0]
                .PictureLocation);
    }
}
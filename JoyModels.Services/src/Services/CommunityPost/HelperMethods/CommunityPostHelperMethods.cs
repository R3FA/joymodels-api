using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPost;
using JoyModels.Models.Enums;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.CommunityPost.HelperMethods;

public static class CommunityPostHelperMethods
{
    private static async Task UpdateCommunityPostUserReview(CommunityPostUserReviewCreateRequest request,
        JoyModelsDbContext context, UserAuthValidation userAuthValidation)
    {
        await context.CommunityPostUserReviews
            .Where(x => x.CommunityPostUuid == request.CommunityPostUuid
                        && x.UserUuid == userAuthValidation.GetUserClaimUuid())
            .ExecuteUpdateAsync(x => x.SetProperty(z => z.ReviewTypeUuid, request.ReviewTypeUuid));

        await context.SaveChangesAsync();
    }

    private static async Task CreateCommunityPostUserReview(CommunityPostUserReviewCreateRequest request,
        JoyModelsDbContext context, UserAuthValidation userAuthValidation)
    {
        await context.CommunityPostUserReviews.AddAsync(
            new CommunityPostUserReview
            {
                Uuid = Guid.NewGuid(),
                UserUuid = userAuthValidation.GetUserClaimUuid(),
                CommunityPostUuid = request.CommunityPostUuid,
                ReviewTypeUuid = request.ReviewTypeUuid,
            });

        await context.SaveChangesAsync();
    }

    public static async Task<JoyModels.Models.Database.Entities.CommunityPost> GetCommunityPostEntity(
        JoyModelsDbContext context,
        Guid communityPostUuid)
    {
        var communityPostEntity = await context.CommunityPosts
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.PostTypeUu)
            .Include(x => x.CommunityPostPictures)
            .FirstOrDefaultAsync(x => x.Uuid == communityPostUuid);

        return communityPostEntity ??
               throw new KeyNotFoundException($"Community post with sent UUID {communityPostUuid} is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.CommunityPost>>
        SearchCommunityPostEntities(
            this CommunityPostSearchRequest request,
            JoyModelsDbContext context)
    {
        var baseQuery = context.CommunityPosts
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.PostTypeUu)
            .Include(x => x.CommunityPostUserReviews)
            .Include(x => x.CommunityPostPictures)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Title))
            baseQuery = baseQuery.Where(x => x.Title.Contains(request.Title));

        baseQuery = request.PostTypeUuid switch
        {
            not null => baseQuery.Where(x => x.PostTypeUuid == request.PostTypeUuid),
            _ => baseQuery
        };

        var resultQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.CommunityPost>.OrderBy(baseQuery, request.OrderBy);

        var communityPostEntities = await PaginationBase<JoyModels.Models.Database.Entities.CommunityPost>.CreateAsync(
            resultQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return communityPostEntities;
    }

    public static async Task<PaginationBase<CommunityPostUserReview>>
        SearchCommunityPostUserReviewEntities(
            this CommunityPostSearchReviewedUsersRequest request,
            JoyModelsDbContext context)
    {
        var baseQuery = context.CommunityPostUserReviews
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.ReviewTypeUu)
            .Where(x => x.CommunityPostUuid == request.CommunityPostUuid)
            .AsQueryable();

        baseQuery = request.CommunityPostReviewType switch
        {
            ModelReviewEnum.Negative => baseQuery.Where(x =>
                x.ReviewTypeUu.ReviewName == nameof(ModelReviewEnum.Negative)),
            ModelReviewEnum.Positive => baseQuery.Where(x =>
                x.ReviewTypeUu.ReviewName == nameof(ModelReviewEnum.Positive)),
            _ => baseQuery
        };

        var resultQuery =
            GlobalHelperMethods<CommunityPostUserReview>.OrderBy(baseQuery, request.OrderBy);

        var communityPostUserReviewEntities = await PaginationBase<CommunityPostUserReview>.CreateAsync(
            resultQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return communityPostUserReviewEntities;
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.CommunityPost>>
        SearchUserLikedCommunityPosts(
            this CommunityPostSearchUserLikedPosts request,
            JoyModelsDbContext context)
    {
        var baseQuery = context.CommunityPostUserReviews
            .AsNoTracking()
            .Include(x => x.ReviewTypeUu)
            .Include(x => x.CommunityPostUu)
            .ThenInclude(cp => cp.UserUu)
            .ThenInclude(u => u.UserRoleUu)
            .Include(x => x.CommunityPostUu)
            .ThenInclude(cp => cp.CommunityPostPictures)
            .Include(x => x.CommunityPostUu)
            .ThenInclude(cp => cp.PostTypeUu)
            .Where(x => x.UserUuid == request.UserUuid)
            .Where(x => x.ReviewTypeUu.ReviewName == nameof(ModelReviewEnum.Positive))
            .Select(x => x.CommunityPostUu)
            .AsQueryable();

        var resultQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.CommunityPost>.OrderBy(baseQuery, request.OrderBy);

        var userLikedCommunityPosts =
            await PaginationBase<JoyModels.Models.Database.Entities.CommunityPost>.CreateAsync(
                resultQuery,
                request.PageNumber,
                request.PageSize,
                request.OrderBy);

        return userLikedCommunityPosts;
    }

    public static async Task<List<string>> SaveCommunityPostPictures(this List<IFormFile> communityPostPictures,
        ModelImageSettingsDetails modelImageSettingsDetails, Guid communityPostUuid)
    {
        var communityPostPicturePaths = new List<string>(communityPostPictures.Count);

        try
        {
            foreach (var communityPostPicture in communityPostPictures)
            {
                var communityPostPictureName =
                    $"community-post-picture-{Guid.NewGuid()}{Path.GetExtension(communityPostPicture.FileName)}";

                var basePath =
                    Directory.CreateDirectory(Path.Combine(modelImageSettingsDetails.SavePath, "community-posts",
                        communityPostUuid.ToString()));
                var communityPostPicturePath = Path.Combine(basePath.FullName, communityPostPictureName);

                await using var stream = new FileStream(communityPostPicturePath, FileMode.Create);
                await communityPostPicture.CopyToAsync(stream);

                communityPostPicturePaths.Add(communityPostPictureName);
            }
        }
        catch (Exception e)
        {
            DeleteCommunityPostPicturesFolderOnException(modelImageSettingsDetails, communityPostUuid);
            throw new ApplicationException($"Failed to save community post picture: {e.Message}");
        }

        return communityPostPicturePaths;
    }

    public static List<CommunityPostPicture> CreateCommunityPostPictureEntityInstances(Guid communityPostUuid,
        List<string> communityPostPicturePaths)
    {
        var communityPostPictureEntities = new List<CommunityPostPicture>(communityPostPicturePaths.Count);

        foreach (var communityPostPicturePath in communityPostPicturePaths)
        {
            communityPostPictureEntities.Add(new CommunityPostPicture()
            {
                Uuid = Guid.NewGuid(),
                CommunityPostUuid = communityPostUuid,
                PictureLocation = communityPostPicturePath,
                CreatedAt = DateTime.Now
            });
        }

        return communityPostPictureEntities;
    }

    public static async Task CreateCommunityPostEntity(this JoyModels.Models.Database.Entities.CommunityPost entity,
        JoyModelsDbContext context)
    {
        await context.CommunityPosts.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public static async Task CreateCommunityPostPictureEntities(
        this List<CommunityPostPicture>? entities,
        JoyModelsDbContext context)
    {
        if (entities != null && entities.Count > 0)
        {
            foreach (var entity in entities)
            {
                await context.CommunityPostPictures.AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }
    }

    public static async Task CreateCommunityPostUserReviewEntity(
        this CommunityPostUserReviewCreateRequest request, JoyModelsDbContext context,
        UserAuthValidation userAuthValidation)
    {
        var isAlreadyReviewed = await context.CommunityPostUserReviews
            .AnyAsync(x => x.CommunityPostUuid == request.CommunityPostUuid
                           && x.UserUuid == userAuthValidation.GetUserClaimUuid());

        switch (isAlreadyReviewed)
        {
            case true:
                await UpdateCommunityPostUserReview(request, context, userAuthValidation);
                break;
            case false:
                await CreateCommunityPostUserReview(request, context, userAuthValidation);
                break;
        }
    }

    public static async Task<(List<string> NewlyAddedFiles, List<string> FilesToDeleteAfterCommit)>
        PatchCommunityPostEntity(
            this CommunityPostPatchRequest request,
            ModelImageSettingsDetails modelImageSettingsDetails,
            JoyModelsDbContext context)
    {
        List<string> newlyAddedFiles = [];
        List<string> filesToDeleteAfterCommit = [];

        if (!string.IsNullOrWhiteSpace(request.Title))
            await context.CommunityPosts
                .Where(x => x.Uuid == request.CommunityPostUuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.Title, request.Title));

        if (!string.IsNullOrWhiteSpace(request.Description))
            await context.CommunityPosts
                .Where(x => x.Uuid == request.CommunityPostUuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.Description, request.Description));

        if (request.PostTypeUuid != null)
            await context.CommunityPosts
                .Where(x => x.Uuid == request.CommunityPostUuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.PostTypeUuid, request.PostTypeUuid));

        if (!string.IsNullOrWhiteSpace(request.YoutubeVideoLink))
            await context.CommunityPosts
                .Where(x => x.Uuid == request.CommunityPostUuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.YoutubeVideoLink, request.YoutubeVideoLink));

        if (request.PicturesToAdd is not null && request.PicturesToAdd.Count != 0)
        {
            var communityPostPicturePaths =
                await request.PicturesToAdd.SaveCommunityPostPictures(modelImageSettingsDetails,
                    request.CommunityPostUuid);

            newlyAddedFiles.AddRange(communityPostPicturePaths);

            var communityPostPictureEntities =
                CreateCommunityPostPictureEntityInstances(request.CommunityPostUuid, communityPostPicturePaths);

            await communityPostPictureEntities.CreateCommunityPostPictureEntities(context);
        }

        if (request.PicturesToRemove is not null && request.PicturesToRemove.Count != 0)
        {
            foreach (var fileName in request.PicturesToRemove.Distinct())
            {
                await context.CommunityPostPictures
                    .Where(x => x.CommunityPostUuid == request.CommunityPostUuid
                                && x.PictureLocation == fileName)
                    .ExecuteDeleteAsync();

                var fullPath = Path.Combine(modelImageSettingsDetails.SavePath, "community-posts",
                    request.CommunityPostUuid.ToString(), fileName);

                filesToDeleteAfterCommit.Add(fullPath);
            }
        }

        await context.SaveChangesAsync();

        return (newlyAddedFiles, filesToDeleteAfterCommit);
    }

    public static async Task DeleteCommunityPostUserReview(this CommunityPostUserReviewDeleteRequest request,
        JoyModelsDbContext context, UserAuthValidation userAuthValidation)
    {
        var totalRecords = await context.CommunityPostUserReviews
            .Where(x => x.CommunityPostUuid == request.CommunityPostUuid
                        && x.ReviewTypeUuid == request.ReviewTypeUuid
                        && x.UserUuid == userAuthValidation.GetUserClaimUuid())
            .ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException("You cannot remove a review that you have never reviewed before.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteCommunityPostEntity(JoyModelsDbContext context, Guid communityPostUuid,
        UserAuthValidation userAuthValidation)
    {
        var baseQuery = context.CommunityPosts.AsQueryable();

        baseQuery = userAuthValidation.GetUserClaimRole() switch
        {
            nameof(UserRoleEnum.Admin) or nameof(UserRoleEnum.Root) =>
                baseQuery.Where(x => x.Uuid == communityPostUuid),
            _ => baseQuery.Where(x =>
                x.Uuid == communityPostUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
        };

        var totalRecords = await baseQuery.ExecuteDeleteAsync();

        if (totalRecords <= 0)
            throw new KeyNotFoundException("Community post either doesn't exist or isn't under your ownership.");

        await context.SaveChangesAsync();
    }

    public static void DeleteCommunityPostPicturesFolderOnException(ModelImageSettingsDetails modelImageSettingsDetails,
        Guid communityPostUuid)
    {
        var folder = Path.Combine(modelImageSettingsDetails.SavePath, "community-posts", communityPostUuid.ToString());
        if (Directory.Exists(folder)) Directory.Delete(folder, true);
    }
}
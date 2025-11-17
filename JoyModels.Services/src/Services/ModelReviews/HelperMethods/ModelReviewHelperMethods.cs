using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;
using ModelReviewEnum = JoyModels.Models.Enums.ModelReview;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.Services.Services.ModelReviews.HelperMethods;

public static class ModelReviewHelperMethods
{
    public static async Task<ModelReview> GetModelReviewEntity(JoyModelsDbContext context, Guid modelReviewUuid)
    {
        var modelReviewEntity = await context.ModelReviews
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.ModelUu)
            .Include(x => x.ModelUu.UserUu)
            .Include(x => x.ModelUu.UserUu.UserRoleUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelUu.ModelPictures)
            .Include(x => x.ReviewTypeUu)
            .FirstOrDefaultAsync(x => x.Uuid == modelReviewUuid);

        return modelReviewEntity ??
               throw new KeyNotFoundException("Model review entity with sent values is not found.");
    }

    public static async Task<PaginationBase<ModelReview>> SearchModelReviewEntities(JoyModelsDbContext context,
        ModelReviewSearchRequest request)
    {
        var baseQuery = context.ModelReviews
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.ModelUu)
            .Include(x => x.ModelUu.UserUu)
            .Include(x => x.ModelUu.UserUu.UserRoleUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelUu.ModelPictures)
            .Include(x => x.ReviewTypeUu)
            .AsQueryable();

        baseQuery = request.ModelReviewType.ToString() switch
        {
            nameof(ModelReviewEnum.Positive) => baseQuery
                .Where(x => string.Equals(x.ReviewTypeUu.ReviewName, nameof(ModelReviewEnum.Positive))),
            nameof(ModelReviewEnum.Negative) => baseQuery
                .Where(x => string.Equals(x.ReviewTypeUu.ReviewName, nameof(ModelReviewEnum.Negative))),
            _ => baseQuery
        };

        var resultQuery = GlobalHelperMethods<ModelReview>.OrderBy(baseQuery, request.OrderBy);

        var modelReviewEntities = await PaginationBase<ModelReview>.CreateAsync(
            resultQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return modelReviewEntities;
    }

    public static async Task CreateModelReviewEntity(this ModelReview modelReviewEntity, JoyModelsDbContext context)
    {
        await context.ModelReviews.AddAsync(modelReviewEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchModelEntity(
        this ModelReviewPatchRequest request,
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation)
    {
        if (request.ModelReviewTypeUuid != null)
            await context.ModelReviews
                .Where(x => x.Uuid == request.ModelReviewUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.ReviewTypeUuid, request.ModelReviewTypeUuid));

        if (!string.IsNullOrWhiteSpace(request.ModelReviewText))
            await context.ModelReviews
                .Where(x => x.Uuid == request.ModelReviewUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.ReviewText, request.ModelReviewText));

        await context.SaveChangesAsync();
    }

    public static async Task DeleteModelReview(JoyModelsDbContext context, Guid modelReviewUuid,
        UserAuthValidation userAuthValidation)
    {
        var baseQuery = context.ModelReviews.AsQueryable();

        baseQuery = userAuthValidation.GetUserClaimRole() switch
        {
            nameof(UserRoleEnum.Admin) or nameof(UserRoleEnum.Root) => baseQuery.Where(x => x.Uuid == modelReviewUuid),
            _ => baseQuery.Where(x => x.Uuid == modelReviewUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
        };

        var totalCount = await baseQuery.ExecuteDeleteAsync();

        if (totalCount <= 0)
            throw new KeyNotFoundException("Model review either doesn't exist or isn't under you ownership.");

        await context.SaveChangesAsync();
    }
}
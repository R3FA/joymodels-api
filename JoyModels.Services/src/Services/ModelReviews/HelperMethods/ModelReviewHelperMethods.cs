using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

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

    public static async Task CreateModelReviewEntity(this ModelReview modelReviewEntity, JoyModelsDbContext context)
    {
        await context.ModelReviews.AddAsync(modelReviewEntity);
        await context.SaveChangesAsync();
    }
}
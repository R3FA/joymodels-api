using JoyModels.Models.Database;
using Microsoft.EntityFrameworkCore;
using ModelAvailabilityEnum = JoyModels.Models.Enums.ModelAvailability;

namespace JoyModels.Services.Services.ModelFaqSection.HelperMethods;

public static class ModelFaqSectionHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.ModelFaqSection> GetModelFaqSectionEntity(
        JoyModelsDbContext context, Guid modelFaqSectionUuid)
    {
        var modelFaqSectionEntity = await context.ModelFaqSections
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
            .Include(x => x.ParentMessage)
            .ThenInclude(p => p.UserUu)
            .ThenInclude(pu => pu.UserRoleUu)
            .Include(x => x.ParentMessage)
            .ThenInclude(p => p.ModelUu)
            .ThenInclude(pm => pm.ModelPictures)
            .Include(x => x.Replies)
            .ThenInclude(r => r.UserUu)
            .ThenInclude(ru => ru.UserRoleUu)
            .Include(x => x.Replies)
            .ThenInclude(r => r.ModelUu)
            .Where(x => string.Equals(x.ModelUu.ModelAvailabilityUu.AvailabilityName,
                nameof(ModelAvailabilityEnum.Public)))
            .FirstOrDefaultAsync(x => x.Uuid == modelFaqSectionUuid);

        return modelFaqSectionEntity ??
               throw new KeyNotFoundException("Model FAQ Section with sent values is not found.");
    }

    public static async Task CreateModelFaqSectionEntity(
        this JoyModels.Models.Database.Entities.ModelFaqSection modelFaqSectionEntity,
        JoyModelsDbContext context)
    {
        await context.ModelFaqSections.AddAsync(modelFaqSectionEntity);
        await context.SaveChangesAsync();
    }
}
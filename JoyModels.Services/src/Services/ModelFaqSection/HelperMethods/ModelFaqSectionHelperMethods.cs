using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.Enums;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.ModelFaqSection.HelperMethods;

public static class ModelFaqSectionHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.ModelFaqSection> GetModelFaqSectionEntity(
        JoyModelsDbContext context, Guid modelFaqSectionUuid)
    {
        var modelFaqSectionEntity = await context.ModelFaqSections
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.ModelUu)
            .ThenInclude(y => y.UserUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ParentMessage)
            .ThenInclude(y => y!.UserUu)
            .Include(x => x.Replies)
            .ThenInclude(y => y.UserUu)
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

    public static async Task PatchModelFaqSectionEntity(this ModelFaqSectionPatchRequest request,
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation)
    {
        var totalRecords = await context.ModelFaqSections
            .Where(x => x.Uuid == request.ModelFaqSectionUuid
                        && x.ModelUuid == request.ModelUuid
                        && x.UserUuid == userAuthValidation.GetUserClaimUuid())
            .ExecuteUpdateAsync(x => x.SetProperty(z => z.MessageText, request.MessageText));

        if (totalRecords <= 0)
            throw new KeyNotFoundException("No question or answer found for editing.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteModelFaqSectionEntity(this ModelFaqSectionDeleteRequest request,
        JoyModelsDbContext context, UserAuthValidation userAuthValidation)
    {
        var baseQuery = context.ModelFaqSections.AsQueryable();

        baseQuery = userAuthValidation.GetUserClaimRole() switch
        {
            nameof(UserRoleEnum.Admin) or nameof(UserRoleEnum.Root) => baseQuery.Where(x =>
                x.Uuid == request.ModelFaqSectionUuid),
            _ => baseQuery.Where(x =>
                x.Uuid == request.ModelFaqSectionUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
        };

        var totalCount = await baseQuery.ExecuteDeleteAsync();

        if (totalCount <= 0)
            throw new KeyNotFoundException("ModelFaqSectionEntity either doesn't exist or isn't under your ownership.");

        await context.SaveChangesAsync();
    }
}
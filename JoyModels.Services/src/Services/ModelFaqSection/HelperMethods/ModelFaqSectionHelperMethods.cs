using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelFaqSection;
using JoyModels.Models.Enums;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
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
            .ThenInclude(y => y.UserRoleUu)
            .Include(x => x.ModelUu)
            .ThenInclude(y => y.UserUu)
            .ThenInclude(y => y.UserRoleUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelCategories)
            .ThenInclude(mc => mc.CategoryUu)
            .Include(x => x.ModelUu.ModelPictures)
            .Include(x => x.ParentMessage)
            .ThenInclude(y => y!.UserUu)
            .ThenInclude(u => u.UserRoleUu)
            .Include(x => x.Replies)
            .ThenInclude(y => y.UserUu)
            .ThenInclude(u => u.UserRoleUu)
            .FirstOrDefaultAsync(x => x.Uuid == modelFaqSectionUuid);

        return modelFaqSectionEntity ??
               throw new KeyNotFoundException("Model FAQ Section with sent values is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.ModelFaqSection>> SearchModelFaqEntities(
        JoyModelsDbContext context,
        ModelFaqSectionSearchRequest request)
    {
        var baseQuery = context.ModelFaqSections
            .AsNoTracking()
            .Include(x => x.UserUu)
            .ThenInclude(y => y.UserRoleUu)
            .Include(x => x.ModelUu)
            .ThenInclude(y => y.UserUu)
            .ThenInclude(y => y.UserRoleUu)
            .Include(x => x.ModelUu.ModelAvailabilityUu)
            .Include(x => x.ModelUu.ModelCategories)
            .ThenInclude(mc => mc.CategoryUu)
            .Include(x => x.ModelUu.ModelPictures)
            .Include(x => x.ParentMessage)
            .ThenInclude(y => y!.UserUu)
            .ThenInclude(u => u.UserRoleUu)
            .Include(x => x.Replies)
            .ThenInclude(y => y.UserUu)
            .ThenInclude(u => u.UserRoleUu)
            .Where(x => x.ModelUuid == request.ModelUuid &&
                        x.ParentMessageUuid == null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.FaqMessage))
            baseQuery = baseQuery.Where(x => x.MessageText.Contains(request.FaqMessage));

        var resultQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.ModelFaqSection>.OrderBy(baseQuery, request.OrderBy);

        var modelEntities = await PaginationBase<JoyModels.Models.Database.Entities.ModelFaqSection>.CreateAsync(
            resultQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);

        return modelEntities;
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
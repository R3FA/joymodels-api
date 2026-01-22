using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostQuestionSection;
using JoyModels.Models.Enums;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.CommunityPostQuestionSection.HelperMethods;

public static class CommunityPostQuestionSectionHelperMethods
{
    public static async Task<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>
        GetCommunityPostQuestionSectionEntity(
            JoyModelsDbContext context, Guid communityPostQuestionSectionUuid)
    {
        var communityPostQuestionSectionEntity = await context.CommunityPostQuestionSections
            .AsNoTracking()
            .Include(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.CommunityPostUu)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.CommunityPostUu)
            .ThenInclude(x => x.PostTypeUu)
            .Include(x => x.ParentMessage)
            .ThenInclude(x => x!.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Replies)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .FirstOrDefaultAsync(x => x.Uuid == communityPostQuestionSectionUuid);

        return communityPostQuestionSectionEntity ??
               throw new KeyNotFoundException(
                   $"Community Post Question Section with sent UUID {communityPostQuestionSectionUuid} is not found.");
    }

    public static async Task<PaginationBase<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>>
        SearchCommunityPostEntities(JoyModelsDbContext context,
            CommunityPostQuestionSectionSearchRequest request)
    {
        var baseQuery = context.CommunityPostQuestionSections
            .AsNoTracking()
            .Include(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.CommunityPostUu)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.CommunityPostUu)
            .ThenInclude(x => x.PostTypeUu)
            .Include(x => x.ParentMessage)
            .ThenInclude(x => x!.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Replies)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Where(x => x.CommunityPostUuid == request.CommunityPostUuid)
            .AsQueryable();

        var resultQuery =
            GlobalHelperMethods<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>.OrderBy(baseQuery,
                request.OrderBy);

        var communityPostQuestionEntities =
            await PaginationBase<JoyModels.Models.Database.Entities.CommunityPostQuestionSection>.CreateAsync(
                resultQuery,
                request.PageNumber,
                request.PageSize,
                request.OrderBy);

        return communityPostQuestionEntities;
    }

    public static async Task CreateCommunityPostQuestionSectionEntity(
        this JoyModels.Models.Database.Entities.CommunityPostQuestionSection communityPostQuestionSectionEntity,
        JoyModelsDbContext context)
    {
        await context.CommunityPostQuestionSections.AddAsync(communityPostQuestionSectionEntity);
        await context.SaveChangesAsync();
    }

    public static async Task PatchCommunityPostQuestionSectionEntity(
        this CommunityPostQuestionSectionPatchRequest request, JoyModelsDbContext context,
        UserAuthValidation userAuthValidation)
    {
        var totalRecords = await context.CommunityPostQuestionSections
            .Where(x => x.Uuid == request.CommunityPostQuestionSectionUuid
                        && x.UserUuid == userAuthValidation.GetUserClaimUuid())
            .ExecuteUpdateAsync(x => x.SetProperty(z => z.MessageText, request.MessageText));

        if (totalRecords <= 0)
            throw new KeyNotFoundException("No question or answer found for editing.");

        await context.SaveChangesAsync();
    }

    public static async Task DeleteCommunityPostQuestionSectionEntity(JoyModelsDbContext context,
        UserAuthValidation userAuthValidation, Guid communityPostQuestionSectionUuid)
    {
        var baseQuery = context.CommunityPostQuestionSections.AsQueryable();

        baseQuery = userAuthValidation.GetUserClaimRole() switch
        {
            nameof(UserRoleEnum.Admin) or nameof(UserRoleEnum.Root) => baseQuery.Where(x =>
                x.Uuid == communityPostQuestionSectionUuid),
            _ => baseQuery.Where(x =>
                x.Uuid == communityPostQuestionSectionUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
        };

        var totalCount = await baseQuery.ExecuteDeleteAsync();

        if (totalCount <= 0)
            throw new KeyNotFoundException(
                "Community post question/answer either doesn't exist or isn't under your ownership.");
    }
}
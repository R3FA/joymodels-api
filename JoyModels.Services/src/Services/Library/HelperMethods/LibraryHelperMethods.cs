using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Library;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.EntityFrameworkCore;
using LibraryEntity = JoyModels.Models.Database.Entities.Library;

namespace JoyModels.Services.Services.Library.HelperMethods;

public static class LibraryHelperMethods
{
    public static async Task<LibraryEntity> GetLibraryEntity(
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation,
        Guid libraryUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var libraryEntity = await context.Libraries
            .AsNoTracking()
            .Include(x => x.Model)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Model.ModelAvailabilityUu)
            .Include(x => x.Model.ModelPictures)
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.Uuid == libraryUuid && x.UserUuid == userUuid);

        return libraryEntity ??
               throw new KeyNotFoundException($"Library item with UUID `{libraryUuid}` does not exist.");
    }

    public static async Task<LibraryEntity> GetLibraryEntityByModelUuid(
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation,
        Guid modelUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var libraryEntity = await context.Libraries
            .AsNoTracking()
            .Include(x => x.Model)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Model.ModelAvailabilityUu)
            .Include(x => x.Model.ModelPictures)
            .Include(x => x.Order)
            .FirstOrDefaultAsync(x => x.ModelUuid == modelUuid && x.UserUuid == userUuid);

        return libraryEntity ??
               throw new KeyNotFoundException($"Model with UUID `{modelUuid}` is not in your library.");
    }

    public static async Task<PaginationBase<LibraryEntity>> SearchLibraryEntities(
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation,
        LibrarySearchRequest request)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        var baseQuery = context.Libraries
            .AsNoTracking()
            .Include(x => x.Model)
            .ThenInclude(x => x.UserUu)
            .ThenInclude(x => x.UserRoleUu)
            .Include(x => x.Model.ModelAvailabilityUu)
            .Include(x => x.Model.ModelPictures)
            .Include(x => x.Order)
            .Where(x => x.UserUuid == userUuid);

        var filteredQuery = request.ModelName switch
        {
            not null => baseQuery.Where(x => x.Model.Name.Contains(request.ModelName)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<LibraryEntity>.OrderBy(filteredQuery, request.OrderBy);

        return await PaginationBase<LibraryEntity>.CreateAsync(
            filteredQuery,
            request.PageNumber,
            request.PageSize,
            request.OrderBy);
    }

    public static async Task<bool> HasUserPurchasedModel(
        JoyModelsDbContext context,
        UserAuthValidation userAuthValidation,
        Guid modelUuid)
    {
        var userUuid = userAuthValidation.GetUserClaimUuid();

        return await context.Libraries
            .AsNoTracking()
            .AnyAsync(x => x.ModelUuid == modelUuid && x.UserUuid == userUuid);
    }
}
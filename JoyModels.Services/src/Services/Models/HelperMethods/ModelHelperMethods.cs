using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.ModelSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.Enums;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Models.HelperMethods;

public static class ModelHelperMethods
{
    public static async Task<Model> GetModelEntityWithAllAvailabilities(JoyModelsDbContext context, Guid modelUuid)
    {
        var modelEntity = await context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.UserUu.UserModelLikes)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(y => y.CategoryUu)
            .Include(x => x.ModelPictures)
            .FirstOrDefaultAsync(x => x.Uuid == modelUuid);

        return modelEntity ?? throw new KeyNotFoundException("3D model with sent values is not found.");
    }

    public static async Task<Model> GetModelEntity(JoyModelsDbContext context, ModelGetByUuidRequest request,
        UserAuthValidation userAuthValidation)
    {
        var baseQuery = context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.UserUu.UserModelLikes)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelPictures)
            .AsQueryable();

        baseQuery = request.ArePrivateModelsSearched switch
        {
            true => baseQuery.Where(x =>
                string.Equals(x.ModelAvailabilityUu.AvailabilityName, nameof(ModelAvailabilityEnum.Hidden))
                && x.UserUuid == userAuthValidation.GetUserClaimUuid()),
            false => baseQuery.Where(x =>
                string.Equals(x.ModelAvailabilityUu.AvailabilityName, nameof(ModelAvailabilityEnum.Public)))
        };

        var modelEntity = await baseQuery.FirstOrDefaultAsync(x => x.Uuid == request.ModelUuid);
        return modelEntity ?? throw new KeyNotFoundException("3D model with sent values is not found.");
    }

    public static async Task<PaginationBase<Model>> SearchModelEntities(JoyModelsDbContext context,
        ModelSearchRequest modelSearchRequestDto, UserAuthValidation userAuthValidation)
    {
        var baseQuery = context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.UserUu.UserModelLikes)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelPictures)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(modelSearchRequestDto.ModelName))
            baseQuery = baseQuery.Where(x => x.Name.Contains(modelSearchRequestDto.ModelName));

        if (!string.IsNullOrWhiteSpace(modelSearchRequestDto.CategoryName))
            baseQuery = baseQuery.Where(x =>
                x.ModelCategories.Any(y => y.CategoryUu.CategoryName.Contains(modelSearchRequestDto.CategoryName)));

        baseQuery = modelSearchRequestDto.ArePrivateUserModelsSearched switch
        {
            true => baseQuery
                .Where(x =>
                    string.Equals(x.ModelAvailabilityUu.AvailabilityName, nameof(ModelAvailabilityEnum.Hidden))
                    && x.UserUuid == userAuthValidation.GetUserClaimUuid()),
            false => baseQuery
                .Where(x =>
                    string.Equals(x.ModelAvailabilityUu.AvailabilityName, nameof(ModelAvailabilityEnum.Public)))
        };

        var resultQuery = GlobalHelperMethods<Model>.OrderBy(baseQuery, modelSearchRequestDto.OrderBy);

        var modelEntities = await PaginationBase<Model>.CreateAsync(
            resultQuery,
            modelSearchRequestDto.PageNumber,
            modelSearchRequestDto.PageSize,
            modelSearchRequestDto.OrderBy);

        return modelEntities;
    }

    public static async Task<PaginationBase<Model>> SearchAdminModelEntities(JoyModelsDbContext context,
        ModelAdminSearchRequest modelAdminSearchRequestDto)
    {
        var baseQuery = context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.UserUu.UserModelLikes)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelPictures)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(modelAdminSearchRequestDto.ModelName))
            baseQuery = baseQuery.Where(x => x.Name.Contains(modelAdminSearchRequestDto.ModelName));

        var resultQuery = GlobalHelperMethods<Model>.OrderBy(baseQuery, modelAdminSearchRequestDto.OrderBy);

        var modelEntities = await PaginationBase<Model>.CreateAsync(
            resultQuery,
            modelAdminSearchRequestDto.PageNumber,
            modelAdminSearchRequestDto.PageSize,
            modelAdminSearchRequestDto.OrderBy);

        return modelEntities;
    }

    public static async Task<PaginationBase<Model>> SearchBestSellingModelEntities(JoyModelsDbContext context,
        ModelBestSellingRequest request)
    {
        var baseQuery = context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.UserUu.UserModelLikes)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelPictures)
            .Where(m => m.ModelAvailabilityUu.AvailabilityName == nameof(ModelAvailabilityEnum.Public))
            .OrderByDescending(m =>
                context.Orders.Count(o => o.ModelUuid == m.Uuid && o.Status == nameof(OrderStatus.Completed)));

        var modelEntities = await PaginationBase<Model>.CreateAsync(
            baseQuery,
            request.PageNumber,
            request.PageSize,
            null);

        return modelEntities;
    }

    public static async Task<PaginationBase<Model>> GetRecommendedModelEntities(
        JoyModelsDbContext context,
        List<Guid> orderedModelUuids,
        ModelRecommendedRequest request)
    {
        var baseQuery = context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.UserUu.UserModelLikes)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelPictures)
            .Where(m => orderedModelUuids.Contains(m.Uuid));

        if (!string.IsNullOrWhiteSpace(request.ModelName))
            baseQuery = baseQuery.Where(x => x.Name.Contains(request.ModelName));

        var models = await baseQuery.ToListAsync();

        var orderedModels = orderedModelUuids
            .Select(uuid => models.FirstOrDefault(m => m.Uuid == uuid))
            .Where(m => m != null)
            .ToList();

        var totalRecords = orderedModels.Count;

        var paginatedModels = orderedModels
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PaginationBase<Model>(
            paginatedModels!,
            totalRecords,
            request.PageNumber,
            request.PageSize,
            null);
    }

    public static async Task CreateModelEntity(this Model modelEntity, JoyModelsDbContext context)
    {
        await context.Models.AddAsync(modelEntity);
        await context.SaveChangesAsync();
    }

    public static async Task CreateModelCategories(this Model modelEntity, JoyModelsDbContext context,
        ModelCreateRequest request)
    {
        foreach (var modelCategoryUuid in request.ModelCategoryUuids)
        {
            var modelCategoryEntity = new ModelCategory
            {
                Uuid = Guid.NewGuid(),
                ModelUuid = modelEntity.Uuid,
                CategoryUuid = modelCategoryUuid
            };

            await context.ModelCategories.AddAsync(modelCategoryEntity);
        }

        await context.SaveChangesAsync();
    }

    public static async Task CreateModelPictures(Model modelEntity, JoyModelsDbContext context,
        List<string> modelPicturePaths)
    {
        foreach (var modelPicturePath in modelPicturePaths)
        {
            var modelPictureEntity = new ModelPicture
            {
                Uuid = Guid.NewGuid(),
                ModelUuid = modelEntity.Uuid,
                PictureLocation = modelPicturePath,
                CreatedAt = DateTime.Now
            };

            await context.ModelPictures.AddAsync(modelPictureEntity);
        }

        await context.SaveChangesAsync();
    }

    public static async Task<List<string>> SaveModelPictures(this List<IFormFile> modelPictures,
        ModelImageSettingsDetails modelImageSettingsDetails, Guid modelUuid)
    {
        var modelPicturePaths = new List<string>(modelPictures.Count);

        try
        {
            foreach (var modelPicture in modelPictures)
            {
                await GlobalValidation.ValidateModelAndCommunityPostPicture(modelPicture, modelImageSettingsDetails);

                var modelPictureName = $"model-picture-{Guid.NewGuid()}{Path.GetExtension(modelPicture.FileName)}";

                var basePath =
                    Directory.CreateDirectory(Path.Combine(modelImageSettingsDetails.SavePath, "models",
                        modelUuid.ToString()));
                var modelPicturePath = Path.Combine(basePath.FullName, modelPictureName);

                await using var stream = new FileStream(modelPicturePath, FileMode.Create);
                await modelPicture.CopyToAsync(stream);

                modelPicturePaths.Add(modelPictureName);
            }
        }
        catch (Exception e)
        {
            DeleteModelPicturesFolderOnException(modelImageSettingsDetails, modelUuid);
            throw new ApplicationException($"Failed to save model picture: {e.Message}");
        }

        return modelPicturePaths;
    }

    public static async Task<string> SaveModel(this IFormFile model,
        ModelSettingsDetails modelSettingsDetails,
        ModelImageSettingsDetails modelImageSettingsDetails,
        Guid modelUuid)
    {
        string modelPath;

        try
        {
            ModelValidation.ValidateModel(model, modelSettingsDetails);

            var modelName = $"model-{Guid.NewGuid()}{Path.GetExtension(model.FileName)}";

            var basePath =
                Directory.CreateDirectory(Path.Combine(modelSettingsDetails.SavePath, "models",
                    modelUuid.ToString()));
            modelPath = Path.Combine(basePath.FullName, modelName);

            await using var stream = new FileStream(modelPath, FileMode.Create);
            await model.CopyToAsync(stream);
        }
        catch (Exception e)
        {
            DeleteModelFolderOnException(modelSettingsDetails, modelUuid);
            DeleteModelPicturesFolderOnException(modelImageSettingsDetails, modelUuid);

            throw new ApplicationException($"Failed to save model: {e.Message}");
        }

        return modelPath;
    }

    public static async Task CreateUserModelLikeEntity(this UserModelLike userModelLikeEntity,
        JoyModelsDbContext context)
    {
        await context.UserModelLikes.AddAsync(userModelLikeEntity);
        await context.SaveChangesAsync();
    }

    public static async Task<(List<string> NewlyAddedFiles, List<string> FilesToDeleteAfterCommit)> PatchModelEntity(
        this ModelPatchRequest request,
        ModelResponse modelResponse,
        ModelImageSettingsDetails modelImageSettingsDetails,
        JoyModelsDbContext context)
    {
        List<string> newlyAddedFiles = [];
        List<string> filesToDeleteAfterCommit = [];

        if (!string.IsNullOrWhiteSpace(request.Name))
            await context.Models
                .Where(x => x.Uuid == modelResponse.Uuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.Name, request.Name));

        if (!string.IsNullOrWhiteSpace(request.Description))
            await context.Models
                .Where(x => x.Uuid == modelResponse.Uuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.Description, request.Description));

        if (request.Price is not null)
            await context.Models
                .Where(x => x.Uuid == modelResponse.Uuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.Price, request.Price));

        if (request.ModelAvailabilityUuid is not null)
            await context.Models
                .Where(x => x.Uuid == modelResponse.Uuid)
                .ExecuteUpdateAsync(x => x.SetProperty(z => z.ModelAvailabilityUuid, request.ModelAvailabilityUuid));

        if (request.ModelCategoriesToDelete is not null && request.ModelCategoriesToDelete.Count != 0)
        {
            foreach (var modelCategoryUuid in request.ModelCategoriesToDelete.Distinct())
            {
                var deletedRows = await context.ModelCategories
                    .Where(x => x.ModelUuid == modelResponse.Uuid && x.CategoryUuid == modelCategoryUuid)
                    .ExecuteDeleteAsync();

                if (deletedRows == 0)
                    throw new ArgumentException($"Cannot delete non existent model category: {modelCategoryUuid}");
            }
        }

        if (request.ModelCategoriesToInsert is not null && request.ModelCategoriesToInsert.Count != 0)
        {
            foreach (var modelCategoryUuid in request.ModelCategoriesToInsert.Distinct())
            {
                var isModelCategoryDuplicated = modelResponse.ModelCategories.Any(x => x.Uuid == modelCategoryUuid);
                if (isModelCategoryDuplicated)
                    throw new ArgumentException($"Cannot insert duplicated model category: {modelCategoryUuid}");

                var modelCategoryEntity = new ModelCategory
                {
                    Uuid = Guid.NewGuid(),
                    ModelUuid = modelResponse.Uuid,
                    CategoryUuid = modelCategoryUuid
                };

                await context.ModelCategories.AddAsync(modelCategoryEntity);
            }
        }

        if (request.ModelPictureToInsert is not null && request.ModelPictureToInsert.Count != 0)
        {
            var maxNumberOfPhotos = modelResponse.ModelPictures.Count + request.ModelPictureToInsert.Count;
            if (maxNumberOfPhotos >= 8)
                throw new ArgumentException("You can upload maximum of 8 pictures per model!");

            var modelPicturePaths =
                await request.ModelPictureToInsert.SaveModelPictures(modelImageSettingsDetails, modelResponse.Uuid);

            newlyAddedFiles.AddRange(modelPicturePaths);

            await CreateModelPictures(new Model { Uuid = modelResponse.Uuid }, context, modelPicturePaths);
        }

        if (request.ModelPictureLocationsToDelete is not null && request.ModelPictureLocationsToDelete.Count != 0)
        {
            foreach (var pictureLocationToDelete in request.ModelPictureLocationsToDelete.Distinct())
            {
                await context.ModelPictures
                    .Where(x => x.ModelUuid == modelResponse.Uuid
                                && x.PictureLocation == pictureLocationToDelete)
                    .ExecuteDeleteAsync();

                var fullPath = Path.Combine(modelImageSettingsDetails.SavePath, "models",
                    modelResponse.Uuid.ToString(), pictureLocationToDelete);

                filesToDeleteAfterCommit.Add(fullPath);
            }
        }

        await context.SaveChangesAsync();

        return (newlyAddedFiles, filesToDeleteAfterCommit);
    }

    public static async Task DeleteUserModelLikeEntity(JoyModelsDbContext context, Guid modelUuid,
        UserAuthValidation userAuthValidation)
    {
        await context.UserModelLikes
            .Where(x => x.UserUuid == userAuthValidation.GetUserClaimUuid()
                        && x.ModelUuid == modelUuid)
            .ExecuteDeleteAsync();
        await context.SaveChangesAsync();
    }

    public static async Task DeleteModel(JoyModelsDbContext context, Guid modelUuid,
        UserAuthValidation userAuthValidation)
    {
        var baseQuery = context.Models.AsQueryable();

        baseQuery = userAuthValidation.GetUserClaimRole() switch
        {
            nameof(UserRoleEnum.Admin) or nameof(UserRoleEnum.Root) => baseQuery.Where(x => x.Uuid == modelUuid),
            _ => baseQuery.Where(x => x.Uuid == modelUuid && x.UserUuid == userAuthValidation.GetUserClaimUuid())
        };

        var totalCount = await baseQuery.ExecuteDeleteAsync();

        if (totalCount <= 0)
            throw new KeyNotFoundException("Model either doesn't exist or isn't under your ownership.");

        await context.SaveChangesAsync();
    }

    public static void DeleteModelFolderOnException(ModelSettingsDetails modelSettingsDetails, Guid modelUuid)
    {
        var modelFolder = Path.Combine(modelSettingsDetails.SavePath, "models", modelUuid.ToString());
        if (Directory.Exists(modelFolder)) Directory.Delete(modelFolder, true);
    }

    public static void DeleteModelPicturesFolderOnException(ModelImageSettingsDetails modelImageSettingsDetails,
        Guid modelUuid)
    {
        var modelFolder = Path.Combine(modelImageSettingsDetails.SavePath, "models", modelUuid.ToString());
        if (Directory.Exists(modelFolder)) Directory.Delete(modelFolder, true);
    }

    public static UserModelLike CreateUserModelLikeObject(Guid modelUuid, UserAuthValidation userAuthValidation)
    {
        return new UserModelLike
        {
            Uuid = Guid.NewGuid(),
            UserUuid = userAuthValidation.GetUserClaimUuid(),
            ModelUuid = modelUuid,
        };
    }
}
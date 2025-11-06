using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.Pagination;
using JoyModels.Services.Extensions;
using JoyModels.Services.Validation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoyModels.Services.Services.Models.HelperMethods;

public static class ModelHelperMethods
{
    public static async Task<Model> GetModelEntity(JoyModelsDbContext context, Guid modelUuid)
    {
        var modelEntity = await context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelPictures)
            .FirstOrDefaultAsync(x => x.Uuid == modelUuid);

        return modelEntity ?? throw new KeyNotFoundException("3D model with sent values is not found.");
    }

    public static async Task<PaginationBase<Model>> SearchModelEntities(JoyModelsDbContext context,
        ModelSearchRequest modelSearchRequestDto)
    {
        var baseQuery = context.Models
            .AsNoTracking()
            .Include(x => x.UserUu)
            .Include(x => x.UserUu.UserRoleUu)
            .Include(x => x.ModelAvailabilityUu)
            .Include(x => x.ModelCategories)
            .ThenInclude(x => x.CategoryUu)
            .Include(x => x.ModelPictures);

        var filteredQuery = modelSearchRequestDto.ModelName switch
        {
            not null => baseQuery.Where(x => x.Name.Contains(modelSearchRequestDto.ModelName)),
            _ => baseQuery
        };

        filteredQuery = GlobalHelperMethods<Model>.OrderBy(filteredQuery, modelSearchRequestDto.OrderBy);

        var modelEntities = await PaginationBase<Model>.CreateAsync(filteredQuery,
            modelSearchRequestDto.PageNumber,
            modelSearchRequestDto.PageSize,
            modelSearchRequestDto.OrderBy);

        return modelEntities;
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

    public static async Task CreateModelPictures(this Model modelEntity, JoyModelsDbContext context,
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
        ImageSettingsDetails imageSettingsDetails, Guid modelUuid)
    {
        var modelPicturePaths = new List<string>(modelPictures.Count);

        try
        {
            foreach (var modelPicture in modelPictures)
            {
                await ModelValidation.ValidateModelPicture(modelPicture, imageSettingsDetails);

                var modelPictureName = $"model-{Guid.NewGuid()}{Path.GetExtension(modelPicture.FileName)}";

                var basePath =
                    Directory.CreateDirectory(Path.Combine(imageSettingsDetails.SavePath, "models",
                        modelUuid.ToString()));
                var modelPicturePath = Path.Combine(basePath.FullName, modelPictureName);

                await using var stream = new FileStream(modelPicturePath, FileMode.Create);
                await modelPicture.CopyToAsync(stream);

                modelPicturePaths.Add(modelPicturePath);
            }
        }
        catch (Exception e)
        {
            foreach (var modelPicturePath in modelPicturePaths)
            {
                if (File.Exists(modelPicturePath)) File.Delete(modelPicturePath);
            }

            throw new ApplicationException($"Failed to save model picture: {e.Message}");
        }

        return modelPicturePaths;
    }
}
using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Models.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.Models;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Services.Services.Models;

public class ModelService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    ImageSettingsDetails imageSettingsDetails)
    : IModelService
{
    public async Task<ModelResponse> GetByUuid(Guid modelUuid)
    {
        var modelEntity = await ModelHelperMethods.GetModelEntity(context, modelUuid);
        return mapper.Map<ModelResponse>(modelEntity);
    }

    public async Task<PaginationResponse<ModelResponse>> Search(ModelSearchRequest request)
    {
        request.ValidateModelSearchArguments();

        var modelEntities = await ModelHelperMethods.SearchModelEntities(context, request);

        return mapper.Map<PaginationResponse<ModelResponse>>(modelEntities);
    }

    public async Task<ModelResponse> Create(ModelCreateRequest request)
    {
        request.ValidateModelCreateArguments();

        var modelEntity = mapper.Map<Model>(request);
        modelEntity.UserUuid = userAuthValidation.GetAuthUserUuid();

        var test = SaveFile(request.Pictures, modelEntity.Uuid);

        var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await modelEntity.CreateModelEntity(context);
            await modelEntity.CreateModelCategories(context, request);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            throw new TransactionException(ex.InnerException!.Message);
        }

        return await GetByUuid(modelEntity.Uuid);
    }

    public async Task<string> SaveFile(IFormFile file, Guid modelUuid)
    {
        ModelValidation.ValidateModelPictureExtension(file, imageSettingsDetails);

        var modelPictureName = $"model-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var basePath =
            Directory.CreateDirectory(Path.Combine(imageSettingsDetails.SavePath, "models", modelUuid.ToString()));

        var filePath = Path.Combine(basePath.FullName, modelPictureName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return filePath;
    }
}
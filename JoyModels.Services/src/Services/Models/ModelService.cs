using System.Transactions;
using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.Models.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.Models;

namespace JoyModels.Services.Services.Models;

public class ModelService(JoyModelsDbContext context, IMapper mapper, UserAuthValidation userAuthValidation)
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
}
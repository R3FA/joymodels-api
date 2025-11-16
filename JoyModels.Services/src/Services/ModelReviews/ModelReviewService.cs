using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.Database.Entities;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.RequestTypes.Models;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviews;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.ModelReviews.HelperMethods;
using JoyModels.Services.Services.Models;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.ModelReviews;

namespace JoyModels.Services.Services.ModelReviews;

public class ModelReviewService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    IModelService modelService)
    : IModelReviewService
{
    public async Task<ModelReviewResponse> GetByUuid(Guid modelReviewUuid)
    {
        var modelReviewEntity = await ModelReviewHelperMethods.GetModelReviewEntity(context, modelReviewUuid);

        return mapper.Map<ModelReviewResponse>(modelReviewEntity);
    }

    public async Task<PaginationResponse<ModelReviewResponse>> Search(ModelReviewSearchRequest request)
    {
        var modelReviewEntities = await ModelReviewHelperMethods.SearchModelReviewEntities(context, request);

        return mapper.Map<PaginationResponse<ModelReviewResponse>>(modelReviewEntities);
    }

    public async Task<ModelReviewResponse> Create(ModelReviewCreateRequest request)
    {
        var modelResponse = await modelService.GetByUuid(new ModelGetByUuidRequest
            { ArePrivateModelsSearched = false, ModelUuid = request.ModelUuid });

        request.ValidateModelReviewCreateArguments(modelResponse, userAuthValidation.GetUserClaimUuid());
        await ModelReviewValidation.ValidateDuplicatedModelReviews(context, modelResponse.Uuid,
            userAuthValidation.GetUserClaimUuid());

        var modelReviewEntity = mapper.Map<ModelReview>(request);
        modelReviewEntity.UserUuid = userAuthValidation.GetUserClaimUuid();

        await modelReviewEntity.CreateModelReviewEntity(context);

        return await GetByUuid(modelReviewEntity.Uuid);
    }
}
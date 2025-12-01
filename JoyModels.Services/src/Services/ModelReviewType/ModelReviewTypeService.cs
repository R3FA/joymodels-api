using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.ModelReviewsType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.ModelReviewType.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.ModelReviewType;

public class ModelReviewTypeService(
    JoyModelsDbContext context,
    IMapper mapper) : IModelReviewTypeService
{
    public async Task<ModelReviewTypeResponse> GetByUuid(Guid modelReviewTypeUuid)
    {
        var modelReviewTypeEntity =
            await ModelReviewTypeHelperMethods.GetModelReviewTypeEntity(context, modelReviewTypeUuid);

        return mapper.Map<ModelReviewTypeResponse>(modelReviewTypeEntity);
    }

    public async Task<PaginationResponse<ModelReviewTypeResponse>> Search(ModelReviewTypeSearchRequest request)
    {
        request.ValidateModelReviewTypeSearchArguments();

        var modelReviewTypeEntities =
            await ModelReviewTypeHelperMethods.SearchModelReviewTypeEntities(context, request);

        return mapper.Map<PaginationResponse<ModelReviewTypeResponse>>(modelReviewTypeEntities);
    }
}
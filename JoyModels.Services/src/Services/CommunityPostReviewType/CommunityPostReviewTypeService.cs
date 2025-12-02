using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostReviewType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.CommunityPostReviewType.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.CommunityPostReviewType;

public class CommunityPostReviewTypeService(JoyModelsDbContext context, IMapper mapper)
    : ICommunityPostReviewTypeService
{
    public async Task<CommunityPostReviewTypeResponse> GetByUuid(Guid communityPostReviewTypeUuid)
    {
        var communityPostReviewTypeEntity =
            await CommunityPostReviewTypeHelperMethods.GetCommunityPostReviewTypeEntity(context,
                communityPostReviewTypeUuid);

        return mapper.Map<CommunityPostReviewTypeResponse>(communityPostReviewTypeEntity);
    }

    public async Task<PaginationResponse<CommunityPostReviewTypeResponse>> Search(
        CommunityPostReviewTypeSearchRequest request)
    {
        request.ValidateCommunityPostReviewTypeSearchArguments();

        var communityPostReviewTypeEntities = await CommunityPostReviewTypeHelperMethods
            .SearchCommunityPostReviewTypeEntities(context, request);

        return mapper.Map<PaginationResponse<CommunityPostReviewTypeResponse>>(communityPostReviewTypeEntities);
    }

    public async Task<CommunityPostReviewTypeResponse> Create(CommunityPostReviewTypeCreateRequest request)
    {
        request.ValidateCommunityPostReviewTypeCreateArguments();

        var communityPostReviewTypeEntity =
            mapper.Map<JoyModels.Models.Database.Entities.CommunityPostReviewType>(request);
        await communityPostReviewTypeEntity.CreateCommunityPostReviewTypeEntity(context);

        return await GetByUuid(communityPostReviewTypeEntity.Uuid);
    }

    public async Task<CommunityPostReviewTypeResponse> Patch(CommunityPostReviewTypePatchRequest request)
    {
        request.ValidateCommunityPostReviewTypePatchArguments();

        await request.PatchCommunityPostReviewTypeEntity(context);

        return await GetByUuid(request.CommunityPostReviewTypeUuid);
    }

    public async Task Delete(Guid communityPostReviewTypeUuid)
    {
        await CommunityPostReviewTypeHelperMethods.DeleteCommunityPostReviewTypeEntity(context,
            communityPostReviewTypeUuid);
    }
}
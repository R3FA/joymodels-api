using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.CommunityPostType;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Services.Services.CommunityPostType.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.CommunityPostType;

public class CommunityPostTypeService(JoyModelsDbContext context, IMapper mapper) : ICommunityPostTypeService
{
    public async Task<CommunityPostTypeResponse> GetByUuid(Guid communityPostTypeUuid)
    {
        var communityPostTypeEntity =
            await CommunityPostTypeHelperMethods.GetCommunityPostTypeEntity(context, communityPostTypeUuid);

        return mapper.Map<CommunityPostTypeResponse>(communityPostTypeEntity);
    }

    public async Task<PaginationResponse<CommunityPostTypeResponse>> Search(CommunityPostTypeSearchRequest request)
    {
        request.ValidateCommunityPostTypeSearchArguments();

        var communityPostTypeEntities =
            await CommunityPostTypeHelperMethods.SearchCommunityPostTypeEntities(context, request);

        return mapper.Map<PaginationResponse<CommunityPostTypeResponse>>(communityPostTypeEntities);
    }

    public async Task<CommunityPostTypeResponse> Create(CommunityPostTypeCreateRequest request)
    {
        request.ValidateCommunityPostCreateArguments();

        var communityPostTypeEntity = mapper.Map<JoyModels.Models.Database.Entities.CommunityPostType>(request);

        await communityPostTypeEntity.CreateCommunityPostTypeEntity(context);

        return mapper.Map<CommunityPostTypeResponse>(communityPostTypeEntity);
    }

    public async Task<CommunityPostTypeResponse> Patch(CommunityPostTypePatchRequest request)
    {
        request.ValidateCommunityPostPatchArguments();

        await request.PatchCommunityPostTypeEntity(context);

        return await GetByUuid(request.PostTypeUuid);
    }

    public async Task Delete(Guid communityPostTypeUuid)
    {
        await CommunityPostTypeHelperMethods.DeleteCommunityPostTypeEntity(context, communityPostTypeUuid);
    }
}
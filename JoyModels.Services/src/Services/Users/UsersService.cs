using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.ImageSettings;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Pagination;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Services.Services.Users.HelperMethods;
using JoyModels.Services.Validation;
using JoyModels.Services.Validation.Users;

namespace JoyModels.Services.Services.Users;

public class UsersService(
    JoyModelsDbContext context,
    IMapper mapper,
    UserAuthValidation userAuthValidation,
    UserImageSettingsDetails userImageSettingsDetails)
    : IUsersService
{
    public async Task<UsersResponse> GetByUuid(Guid userUuid)
    {
        var userEntity = await UsersHelperMethods.GetUserEntity(context, userUuid);
        return mapper.Map<UsersResponse>(userEntity);
    }

    public async Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request)
    {
        request.ValidateUserSearchArguments();

        var userEntities = await UsersHelperMethods.SearchUserEntities(context, request);
        return mapper.Map<PaginationResponse<UsersResponse>>(userEntities);
    }

    public async Task<UsersResponse> Patch(Guid userUuid, UsersPatchRequest request)
    {
        userAuthValidation.ValidateUserAuthRequest(userUuid);
        userAuthValidation.ValidateRequestUuids(userUuid, request.UserUuid);
        request.ValidateUserPatchArguments();
        await request.ValidateUserPatchArgumentsDuplicatedFields(context);

        var userResponse = await GetByUuid(userUuid);

        await request.PatchUserEntity(context, userResponse, userImageSettingsDetails);

        return await GetByUuid(userUuid);
    }

    public async Task Delete(Guid userUuid)
    {
        await UsersHelperMethods.DeleteUserEntity(context, userUuid, userAuthValidation);
    }
}
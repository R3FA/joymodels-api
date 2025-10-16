using AutoMapper;
using JoyModels.Models.Database;
using JoyModels.Models.DataTransferObjects.RequestTypes.Users;
using JoyModels.Models.DataTransferObjects.ResponseTypes;
using JoyModels.Models.DataTransferObjects.ResponseTypes.Users;
using JoyModels.Services.Services.Users.HelperMethods;
using JoyModels.Services.Validation;

namespace JoyModels.Services.Services.Users;

public class UsersService(JoyModelsDbContext context, IMapper mapper, UserAuthValidation userAuthValidation)
    : IUsersService
{
    public async Task<UsersResponse> GetByUuid(Guid userUuid)
    {
        var userEntity = await UsersHelperMethods.GetUserEntity(context, userUuid);
        var userResponse = mapper.Map<UsersResponse>(userEntity);

        return userResponse;
    }

    public async Task<PaginationResponse<UsersResponse>> Search(UsersSearchRequest request)
    {
        throw new NotImplementedException();
    }
}
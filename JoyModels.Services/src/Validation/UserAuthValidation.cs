using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Services.Validation;

public sealed class UserAuthValidation(IHttpContextAccessor httpContext)
{
    private Guid GetAuthUserUuid()
    {
        var userNameIdentifierClaim =
            httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userNameIdentifierClaim != null && Guid.TryParse(userNameIdentifierClaim.Value, out var userUuid))
            return userUuid;

        throw new ApplicationException("Error getting user uuid from access token!");
    }

    public void ValidateUserAuthRequest(Guid requestUserUuid)
    {
        if (GetAuthUserUuid() != requestUserUuid)
            throw new ApplicationException("You are not authorized for this request.");
    }

    public void ValidateUserRequestUuids(Guid queryUserUuid, Guid requestUserUuid)
    {
        if (queryUserUuid != requestUserUuid)
            throw new ArgumentException("User uuid query parameter doesn't match the user uuid in request body.");
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Services.Validation;

public sealed class UserAuthValidation(IHttpContextAccessor httpContext)
{
    private IEnumerable<Claim> GetUserClaim()
    {
        return httpContext.HttpContext.User.Claims;
    }

    public Guid GetUserClaimUuid()
    {
        var userClaim = GetUserClaim();

        var userClaimUuid = userClaim
            .Where(x => x.Type == ClaimTypes.NameIdentifier)
            .Select(x => x.Value)
            .FirstOrDefault();

        return userClaimUuid is null
            ? throw new ApplicationException("Cannot get user claim uuid from access token!")
            : Guid.Parse(userClaimUuid);
    }

    public string GetUserClaimRole()
    {
        var userClaim = GetUserClaim();

        var userClaimRole = userClaim
            .Where(x => x.Type == ClaimTypes.Role)
            .Select(x => x.Value)
            .FirstOrDefault();

        return userClaimRole ?? throw new ApplicationException("Cannot get user claim role from access token!");
    }

    public void ValidateUserAuthRequest(Guid routeUserUuid)
    {
        if (GetUserClaimUuid() != routeUserUuid)
            throw new ApplicationException("You are not authorized for this request.");
    }

    public void ValidateRequestUuids(Guid routeUuid, Guid requestUuid)
    {
        if (routeUuid != requestUuid)
            throw new ArgumentException("Route uuid doesn't match with request uuid.");
    }
}
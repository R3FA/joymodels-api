using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace JoyModels.Services.Validation;

public sealed class UserAuthValidation
{
    private readonly IHttpContextAccessor _httpContext;

    public UserAuthValidation(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    public Guid GetAuthUserUuid()
    {
        var userRefUuid = _httpContext.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userRefUuid != null && Guid.TryParse(userRefUuid.Value, out var userUuid))
            return userUuid;

        throw new ApplicationException("Error getting user uuid from access token!");
    }
}
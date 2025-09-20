using System.Text;
using JoyModels.Models.DataTransferObjects.Sso;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserRoleEnum = JoyModels.Models.Enums.UserRole;

namespace JoyModels.API.Setups;

public static class JwtSetup
{
    public static IServiceCollection RegisterJwtServices(this IServiceCollection services, IConfiguration configuration)
    {
        var ssoJwtDetails = RegisterJwtDetails(configuration);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = ssoJwtDetails.JwtIssuer,
                    ValidateAudience = true,
                    ValidAudience = ssoJwtDetails.JwtAudience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ssoJwtDetails.JwtSigningKey))
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("VerifiedUsers", policy =>
                policy.RequireRole(
                    nameof(UserRoleEnum.User),
                    nameof(UserRoleEnum.Helper),
                    nameof(UserRoleEnum.Admin),
                    nameof(UserRoleEnum.Root)
                ));
        });

        return services;
    }

    public static SsoJwtDetails RegisterJwtDetails(IConfiguration configuration)
    {
        var jwtDetails = configuration.GetSection("JWT");
        var jwtSigningKey = jwtDetails["SigningKey"];
        var jwtIssuer = jwtDetails["Issuer"];
        var jwtAudience = jwtDetails["Audience"];

        if (string.IsNullOrEmpty(jwtSigningKey)
            || string.IsNullOrEmpty(jwtIssuer)
            || string.IsNullOrEmpty(jwtAudience))
            throw new ApplicationException("JWT Signing Key or Issuer or Audience are not configured!");

        return new SsoJwtDetails()
        {
            JwtSigningKey = jwtSigningKey,
            JwtAudience = jwtAudience,
            JwtIssuer = jwtIssuer
        };
    }
}
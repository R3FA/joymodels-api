using System.Text;
using JoyModels.Models.DataTransferObjects.Jwt;
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ssoJwtDetails.JwtSigningKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("UnverifiedUsers", policy =>
                policy.RequireRole(
                    nameof(UserRoleEnum.Unverified)
                ));

            options.AddPolicy("VerifiedUsers", policy =>
                policy.RequireRole(
                    nameof(UserRoleEnum.User),
                    nameof(UserRoleEnum.Helper),
                    nameof(UserRoleEnum.Admin),
                    nameof(UserRoleEnum.Root)
                ));

            options.AddPolicy("Staff", policy =>
                policy.RequireRole(
                    nameof(UserRoleEnum.Helper),
                    nameof(UserRoleEnum.Admin),
                    nameof(UserRoleEnum.Root)
                ));

            options.AddPolicy("HeadStaff", policy =>
                policy.RequireRole(
                    nameof(UserRoleEnum.Admin),
                    nameof(UserRoleEnum.Root)
                ));
        });

        return services;
    }

    public static JwtClaimDetails RegisterJwtDetails(IConfiguration configuration)
    {
        var jwtDetails = configuration.GetSection("JWT");
        var jwtSigningKey = jwtDetails["SigningKey"];
        var jwtIssuer = jwtDetails["Issuer"];
        var jwtAudience = jwtDetails["Audience"];

        if (string.IsNullOrEmpty(jwtSigningKey)
            || string.IsNullOrEmpty(jwtIssuer)
            || string.IsNullOrEmpty(jwtAudience))
            throw new ApplicationException("JWT Signing Key or Issuer or Audience are not configured!");

        return new JwtClaimDetails()
        {
            JwtSigningKey = jwtSigningKey,
            JwtAudience = jwtAudience,
            JwtIssuer = jwtIssuer
        };
    }
}
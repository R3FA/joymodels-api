using System.Text;
using JoyModels.Models.DataTransferObjects.Jwt;
using JoyModels.Models.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

        if (string.IsNullOrWhiteSpace(jwtSigningKey)
            || string.IsNullOrWhiteSpace(jwtIssuer)
            || string.IsNullOrWhiteSpace(jwtAudience))
            throw new ApplicationException("JWT credentials are not configured!");

        return new JwtClaimDetails()
        {
            JwtSigningKey = jwtSigningKey,
            JwtAudience = jwtAudience,
            JwtIssuer = jwtIssuer
        };
    }
}
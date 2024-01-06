using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.S3;
using Application.Auth.Interfaces;
using Application.Auth.Interfaces.Repositories;
using Application.Auth.Interfaces.Services;
using Application.File.Interfaces;
using Domain.Constants;
using Domain.Helpers;
using Domain.Options;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(ApiConstants.CorsPolicyName, policy =>
            {
                policy.AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(ApiConstants.CorsOrigins);
            });
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(ApiConstants.SwaggerSecurityDefinitionName, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = ApiConstants.SwaggerDescription,
                Name = ApiConstants.SwaggerName,
                Type = SecuritySchemeType.Http,
                BearerFormat = ApiConstants.SwaggerBearerFormat,
                Scheme = ApiConstants.SwaggerScheme
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = ApiConstants.SwaggerScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddJwtAuth(this IServiceCollection services, AWSSettings? awsSettings)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKeyResolver = (_, _, _, parameters) => AuthenticationHelper.GetIssuerSigningKeyResolver(parameters.ValidIssuer),
                ValidIssuer = AuthenticationHelper.GetValidIssuer(awsSettings),
                ValidateIssuer = true,
                ValidateLifetime = true,
                LifetimeValidator = (_, expires, _, _) => expires > DateTime.UtcNow,
                ValidAudience = awsSettings?.ClientId,
                ValidateAudience = true
            };
        });

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtContextAccessor, JwtContextAccessor>();

        return services;
    }
    
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        
        return services;
    }
    
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
        services.Configure<AWSSettings>(configuration.GetSection("AWSSettings"));

        return services;
    }
    
    public static IServiceCollection AddConfigurations(this IServiceCollection services, AWSSettings awsSettings)
    {
        services.AddTransient(_ =>
        {
            var region = RegionEndpoint.GetBySystemName(awsSettings?.RegionName);
            return new AmazonCognitoIdentityProviderClient(region);
        });
        services.AddTransient(_ =>
        {
            var region = RegionEndpoint.GetBySystemName(awsSettings?.RegionName);
            return new AmazonS3Client(region);
        });

        return services;
    }
}
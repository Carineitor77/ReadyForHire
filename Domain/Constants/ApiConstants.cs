namespace Domain.Constants;

public static class ApiConstants
{
    // Controllers
    public const string BaseApiRoute = "api/[controller]/[action]";

    // CORS
    public const string CorsPolicyName = "ReadyForHirePolicy";
    public static readonly string[] CorsOrigins = { "http://localhost:3000" };
    
    // Swagger
    public const string SwaggerSecurityDefinitionName = "Bearer";
    public const string SwaggerDescription = "Authentication Token";
    public const string SwaggerName = "Authorization";
    public const string SwaggerBearerFormat = "JsonWebToken";
    public const string SwaggerScheme = "Bearer";
    
    // Authentication
    public const string AuthSigningKeyResolverPath = "/.well-known/jwks.json";
    public const string CognitoIdp = "https://cognito-idp";
    public const string AmazonUrl = "amazonaws.com";
}
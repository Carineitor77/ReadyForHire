using System.Net;
using Domain.Constants;
using Domain.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Domain.Helpers;

public static class AuthenticationHelper
{
    public static IList<JsonWebKey>? GetIssuerSigningKeyResolver(string validIssuer)
    {
        var webClient = new WebClient();
        var json = webClient.DownloadString(validIssuer + ApiConstants.AuthSigningKeyResolverPath);
        var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json)?.Keys;
        return keys;
    }

    public static string GetValidIssuer(AWSSettings? awsSettings)
    {
        var issuer = $"{ApiConstants.CognitoIdp}.{awsSettings?.RegionName}.{ApiConstants.AmazonUrl}/{awsSettings?.UserPoolId}";
        return issuer;
    }
}
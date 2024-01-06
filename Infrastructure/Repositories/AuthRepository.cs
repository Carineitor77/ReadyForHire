using System.Net;
using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Application.Auth.Interfaces.Repositories;
using Domain.Entities.Auth;
using Domain.Enums;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AmazonCognitoIdentityProviderClient _cognito;
    private readonly AWSSettings _awsSettings;

    public AuthRepository(IOptions<AWSSettings> awsSettings, AmazonCognitoIdentityProviderClient cognito)
    {
        _cognito = cognito;
        _awsSettings = awsSettings.Value;
    }
    
    public async Task<string> SignIn(SignInEntity signInEntity, CancellationToken cancellationToken)
    {
        try
        {
            var cognitoRequest = CreateCognitoSignInRequest(signInEntity);
            var cognitoResponse = await _cognito.AdminInitiateAuthAsync(cognitoRequest, cancellationToken);
            var token = cognitoResponse.AuthenticationResult.IdToken;

            if (token is null)
            {
                throw new ApiException(HttpStatusCode.BadRequest, Errors.Auth.FailedToSignIn);
            }
            
            return token;
        }
        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<bool> SignUp(SignUpEntity signUpEntity, CancellationToken cancellationToken)
    {
        try
        {
            var cognitoRequest = CreateCognitoSignUpRequest(signUpEntity);
            var cognitoResponse = await _cognito.SignUpAsync(cognitoRequest, cancellationToken);
            var isRegistered = cognitoResponse.HttpStatusCode == HttpStatusCode.OK;
            
            if (!isRegistered)
            {
                throw new ApiException(HttpStatusCode.BadRequest, Errors.Auth.FailedToSignUp);
            }
            
            return isRegistered;
        }
        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.BadRequest, e.Message);
        }
    }

    public async Task<bool> VerifyCode(VerifyCodeEntity verifyCodeEntity, CancellationToken cancellationToken)
    {
        try
        {
            var cognitoRequest = CreateCognitoVerifyCodeRequest(verifyCodeEntity);
            var cognitoResponse = await _cognito.ConfirmSignUpAsync(cognitoRequest, cancellationToken);
            var isVerified = cognitoResponse.HttpStatusCode == HttpStatusCode.OK;
            
            if (!isVerified)
            {
                throw new ApiException(HttpStatusCode.BadRequest, Errors.Auth.FailedToVerify);
            }
            
            return isVerified;
        }
        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.BadRequest, e.Message);
        }
    }
    
    private ConfirmSignUpRequest CreateCognitoVerifyCodeRequest(VerifyCodeEntity verifyCodeEntity)
    {
        var secretHash = ComputeSecretHash(verifyCodeEntity.Email);
    
        var cognitoRequest =  new ConfirmSignUpRequest
        {
            ClientId = _awsSettings.ClientId,
            Username = verifyCodeEntity.Email,
            ConfirmationCode = verifyCodeEntity.VerificationCode,
            SecretHash = secretHash
        };
        
        return cognitoRequest;
    }

    private SignUpRequest CreateCognitoSignUpRequest(SignUpEntity signUpEntity)
    {
        var secretHash = ComputeSecretHash(signUpEntity.Email);
        
        var cognitoRequest = new SignUpRequest
        {
            ClientId = _awsSettings.ClientId,
            Password = signUpEntity.Password,
            Username = signUpEntity.Email,
            SecretHash = secretHash,
            UserAttributes = new List<AttributeType>
            {
                new()
                {
                    Name = CognitoAttributes.Role.GetDescription(),
                    Value = CognitoRoles.User.ToString()
                }
            }
        };
        
        return cognitoRequest;
    }

    private AdminInitiateAuthRequest CreateCognitoSignInRequest(SignInEntity signInEntity)
    {
        var secretHash = ComputeSecretHash(signInEntity.Email);
        
        var cognitoRequest = new AdminInitiateAuthRequest
        {
            UserPoolId = _awsSettings.UserPoolId,
            ClientId = _awsSettings.ClientId,
            AuthFlow = AuthFlowType.ADMIN_USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { AuthParameters.UserName.GetDescription(), signInEntity.Email },
                { AuthParameters.Password.GetDescription(), signInEntity.Password },
                { AuthParameters.SecretHash.GetDescription(), secretHash }
            }
        };
        
        return cognitoRequest;
    }
    
    private string ComputeSecretHash(string username)
    {
        var dataToSign = Encoding.UTF8.GetBytes(username + _awsSettings.ClientId);
        var keyBytes = Encoding.UTF8.GetBytes(_awsSettings.ClientSecret);
        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(dataToSign);
        return Convert.ToBase64String(hashBytes);
    }
}
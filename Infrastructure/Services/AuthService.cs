using Application.Auth.Interfaces.Repositories;
using Application.Auth.Interfaces.Services;
using Application.Auth.Mapping;
using Application.Auth.Models;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;

    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }
    
    public async Task<string> SignIn(SignInModel signInModel, CancellationToken cancellationToken)
    {
        var signInEntity = AuthMapping.MapSignInModelToSignInEntity(signInModel);
        var token = await _authRepository.SignIn(signInEntity, cancellationToken);
        return token;
    }

    public async Task<bool> SignUp(SignUpModel signUpModel, CancellationToken cancellationToken)
    {
        var signUpEntity = AuthMapping.MapSignUpModelToSignUpEntity(signUpModel);
        var isRegistered = await _authRepository.SignUp(signUpEntity, cancellationToken);
        return isRegistered;
    }

    public async Task<bool> VerifyCode(VerifyCodeModel verifyCodeModel, CancellationToken cancellationToken)
    {
        var verifyCodeEntity = AuthMapping.MapVerifyCodeModelToVerifyCodeEntity(verifyCodeModel);
        var isVerified = await _authRepository.VerifyCode(verifyCodeEntity, cancellationToken);
        return isVerified;
    }
}
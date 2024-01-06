using Application.Auth.Models;

namespace Application.Auth.Interfaces.Services;

public interface IAuthService
{
    Task<string> SignIn(SignInModel signInModel, CancellationToken cancellationToken);
    Task<bool> SignUp(SignUpModel signUpModel, CancellationToken cancellationToken);
    Task<bool> VerifyCode(VerifyCodeModel verifyCodeModel, CancellationToken cancellationToken);
}
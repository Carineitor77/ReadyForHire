using Domain.Entities.Auth;

namespace Application.Auth.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<string> SignIn(SignInEntity signInEntity, CancellationToken cancellationToken);
    Task<bool> SignUp(SignUpEntity signUpEntity, CancellationToken cancellationToken);
    Task<bool> VerifyCode(VerifyCodeEntity verifyCodeEntity, CancellationToken cancellationToken);

}
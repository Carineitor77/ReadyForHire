using Application.Auth.Commands;
using Application.Auth.Interfaces.Services;
using Application.Auth.Mapping;
using MediatR;

namespace Application.Auth.Handlers;

public class SignInHandler : IRequestHandler<SignInCommand, string>
{
    private readonly IAuthService _authService;

    public SignInHandler(IAuthService authService)
    {
        _authService = authService;
    }
    
    public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var signInModel = AuthMapping.MapSignInCommandToSignInModel(request);
        var token = await _authService.SignIn(signInModel, cancellationToken);
        return token;
    }
}
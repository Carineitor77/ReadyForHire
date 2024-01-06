using Application.Auth.Commands;
using Application.Auth.Interfaces.Services;
using Application.Auth.Mapping;
using MediatR;

namespace Application.Auth.Handlers;

public class SignUpHandler : IRequestHandler<SignUpCommand, bool>
{
    private readonly IAuthService _authService;

    public SignUpHandler(IAuthService authService)
    {
        _authService = authService;
    }
    
    public async Task<bool> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var signUpModel = AuthMapping.MapSignUpCommandToSignUpModel(request);
        var isRegistered = await _authService.SignUp(signUpModel, cancellationToken);
        return isRegistered;
    }
}
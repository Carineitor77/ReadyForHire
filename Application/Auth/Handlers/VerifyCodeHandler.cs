using Application.Auth.Commands;
using Application.Auth.Interfaces.Services;
using Application.Auth.Mapping;
using MediatR;

namespace Application.Auth.Handlers;

public class VerifyCodeHandler: IRequestHandler<VerifyCodeCommand, bool>
{
    private readonly IAuthService _authService;

    public VerifyCodeHandler(IAuthService authService)
    {
        _authService = authService;
    }
    
    public async Task<bool> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
    {
        var verifyCodeModel = AuthMapping.MapVerifyCodeCommandToVerifyCodeModel(request);
        var isVerified = await _authService.VerifyCode(verifyCodeModel, cancellationToken);
        return isVerified;
    }
}
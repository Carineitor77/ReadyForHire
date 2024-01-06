using API.Controllers.Abstract;
using Application.Auth.DTOs;
using Application.Auth.Mapping;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[AllowAnonymous]
public class AuthController : BaseApiController
{
    public AuthController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<IActionResult> SingIn([FromBody] SignInDto signInDto, CancellationToken cancellationToken)
    {
        var command = AuthMapping.MapSignInDtoToSignInCommand(signInDto);
        var token = await Mediator.Send(command, cancellationToken);
        return Ok(token);
    }
    
    [HttpPost]
    public async Task<IActionResult> SingUp([FromBody] SignUpDto signUpDto, CancellationToken cancellationToken)
    {
        var command = AuthMapping.MapSignUpDtoToSignUpCommand(signUpDto);
        var isRegistered = await Mediator.Send(command, cancellationToken);
        return Ok(isRegistered);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto, CancellationToken cancellationToken)
    {
        var command = AuthMapping.MapVerifyCodeDtoToVerifyCodeCommand(verifyCodeDto);
        var isVerified = await Mediator.Send(command, cancellationToken);
        return Ok(isVerified);
    }
}
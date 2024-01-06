using MediatR;

namespace Application.Auth.Commands;

public record SignInCommand(string Email, string Password) : IRequest<string>;
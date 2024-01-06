using MediatR;

namespace Application.Auth.Commands;

public record SignUpCommand(string Email, string Password) : IRequest<bool>;
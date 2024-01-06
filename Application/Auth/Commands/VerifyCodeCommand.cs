using MediatR;

namespace Application.Auth.Commands;

public record VerifyCodeCommand(string Email, string VerificationCode) : IRequest<bool>;
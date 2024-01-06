using Application.Auth.DTOs;
using FluentValidation;

namespace Application.Auth.Validators;

public class VerifyCodeValidator : AbstractValidator<VerifyCodeDto>
{
    public VerifyCodeValidator()
    {
        RuleFor(v => v.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.")
            .MinimumLength(5).WithMessage("Email should be at least 5 characters long.")
            .MaximumLength(50).WithMessage("Email should not exceed 50 characters.");

        RuleFor(v => v.VerificationCode)
            .NotEmpty().WithMessage("Verification code is required.");
    }
}
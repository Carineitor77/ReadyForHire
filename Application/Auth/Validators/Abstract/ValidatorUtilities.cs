using Application.Auth.DTOs.Abstract;
using FluentValidation;

namespace Application.Auth.Validators.Abstract;

public static class ValidatorUtilities
{
    public static void ApplyCommonRules<T>(AbstractValidator<T> validator) where T : BaseAuthDto
    {
        validator.RuleFor(a => a.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.")
            .MinimumLength(5).WithMessage("Email should be at least 5 characters long.")
            .MaximumLength(50).WithMessage("Email should not exceed 50 characters.");

        validator.RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password should be at least 8 characters long.")
            .MaximumLength(50).WithMessage("Password should not exceed 50 characters.");
    }
}
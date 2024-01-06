using Application.Auth.DTOs;
using Application.Auth.Validators.Abstract;
using FluentValidation;

namespace Application.Auth.Validators;

public class SignInValidator : AbstractValidator<SignInDto>
{
    public SignInValidator()
    {
        ValidatorUtilities.ApplyCommonRules(this);
    }
}
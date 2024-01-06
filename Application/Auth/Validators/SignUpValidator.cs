using Application.Auth.DTOs;
using Application.Auth.Validators.Abstract;
using FluentValidation;

namespace Application.Auth.Validators;

public class SignUpValidator : AbstractValidator<SignUpDto>
{
    public SignUpValidator()
    {
        ValidatorUtilities.ApplyCommonRules(this);
    }
}
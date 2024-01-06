using Application.Auth.Commands;
using Application.Auth.DTOs;
using Application.Auth.Models;
using Domain.Entities.Auth;

namespace Application.Auth.Mapping;

public static class AuthMapping
{
    public static SignInEntity MapSignInModelToSignInEntity(SignInModel signInModel)
    {
        var signInEntity = new SignInEntity(signInModel.Email, signInModel.Password);
        return signInEntity;
    }

    public static SignUpEntity MapSignUpModelToSignUpEntity(SignUpModel signUpModel)
    {
        var signUpEntity = new SignUpEntity(signUpModel.Email, signUpModel.Password);
        return signUpEntity;
    }

    public static VerifyCodeEntity MapVerifyCodeModelToVerifyCodeEntity(VerifyCodeModel verifyCodeModel)
    {
        var verifyCodeEntity = new VerifyCodeEntity(verifyCodeModel.Email, verifyCodeModel.VerificationCode);
        return verifyCodeEntity;
    }

    public static SignInModel MapSignInCommandToSignInModel(SignInCommand signInCommand)
    {
        var signInModel = new SignInModel(signInCommand.Email, signInCommand.Password);
        return signInModel;
    }

    public static SignUpModel MapSignUpCommandToSignUpModel(SignUpCommand signUpCommand)
    {
        var signUpModel = new SignUpModel(signUpCommand.Email, signUpCommand.Password);
        return signUpModel;
    }

    public static VerifyCodeModel MapVerifyCodeCommandToVerifyCodeModel(VerifyCodeCommand verifyCodeCommand)
    {
        var verifyCodeModel = new VerifyCodeModel(verifyCodeCommand.Email, verifyCodeCommand.VerificationCode);
        return verifyCodeModel;
    }

    public static SignInCommand MapSignInDtoToSignInCommand(SignInDto signInDto)
    {
        var signInCommand = new SignInCommand(signInDto.Email, signInDto.Password);
        return signInCommand;
    }

    public static SignUpCommand MapSignUpDtoToSignUpCommand(SignUpDto signUpDto)
    {
        var signUpCommand = new SignUpCommand(signUpDto.Email, signUpDto.Password);
        return signUpCommand;
    }

    public static VerifyCodeCommand MapVerifyCodeDtoToVerifyCodeCommand(VerifyCodeDto verifyCodeDto)
    {
        var verifyCodeCommand = new VerifyCodeCommand(verifyCodeDto.Email, verifyCodeDto.VerificationCode);
        return verifyCodeCommand;
    }
}
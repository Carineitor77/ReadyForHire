namespace Application.Auth.Models;

public record VerifyCodeModel(string Email, string VerificationCode);
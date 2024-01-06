namespace Domain.Entities.Auth;

public record VerifyCodeEntity(string Email, string VerificationCode);
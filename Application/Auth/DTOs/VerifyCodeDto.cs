namespace Application.Auth.DTOs;

public record VerifyCodeDto(string Email, string VerificationCode);
using Application.Auth.DTOs.Abstract;

namespace Application.Auth.DTOs;

public record SignInDto(string Email, string Password) : BaseAuthDto(Email, Password);
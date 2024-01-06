using Application.Auth.DTOs.Abstract;

namespace Application.Auth.DTOs;

public record SignUpDto(string Email, string Password) : BaseAuthDto(Email, Password);
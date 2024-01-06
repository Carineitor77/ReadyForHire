using System.Net;
using System.Security.Claims;
using Application.Auth.Interfaces;
using Domain.Enums;
using Domain.Errors;
using Domain.Exceptions;
using Domain.Extensions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class JwtContextAccessor : IJwtContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CognitoRoles GetUserRole()
    {
        var claimType = CognitoAttributes.Role.GetDescription();
        var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);

        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ApiException(HttpStatusCode.Unauthorized, Errors.Auth.RoleIsEmpty);
        }

        var parsedRole = Enum.Parse<CognitoRoles>(role);
        return parsedRole;
    }

    public string GetUserEmail()
    {
        var claimType = CognitoAttributes.Email.GetDescription();
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(claimType);

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ApiException(HttpStatusCode.Unauthorized, Errors.Auth.EmailIsEmpty);
        }

        return email;
    }
}
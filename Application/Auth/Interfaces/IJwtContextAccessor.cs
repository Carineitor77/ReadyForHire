using Domain.Enums;

namespace Application.Auth.Interfaces;

public interface IJwtContextAccessor
{
    CognitoRoles GetUserRole();
    string GetUserEmail();
}
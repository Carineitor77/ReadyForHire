using System.ComponentModel;

namespace Domain.Enums;

public enum CognitoAttributes
{
    [Description("custom:role")]
    Role,
    [Description("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")]
    Email
}
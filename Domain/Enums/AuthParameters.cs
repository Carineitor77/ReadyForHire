using System.ComponentModel;

namespace Domain.Enums;

public enum AuthParameters
{
    [Description("USERNAME")]
    UserName,
    [Description("PASSWORD")]
    Password,
    [Description("SECRET_HASH")]
    SecretHash
}
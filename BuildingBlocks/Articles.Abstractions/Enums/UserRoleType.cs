using System.ComponentModel;

namespace Articles.Abstractions.Enums;

public enum UserRoleType
{
    [Description("Editorial Office")]
    EOF = 1,
    [Description("Author")]
    AUT = 11,
    [Description("Corresponding Author")]
    CORAUT = 12,
}

public static class Roles
{
    public const string EOF = nameof(UserRoleType.EOF);
    public const string AUT = nameof(UserRoleType.AUT);
    public const string CORAUT = nameof(UserRoleType.CORAUT);
}

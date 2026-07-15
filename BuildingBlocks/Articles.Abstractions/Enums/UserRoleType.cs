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

using Articles.Abstractions.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Role;

public class Role : IdentityRole<int>
{
    public required UserRoleType Type { get; set; }
    public required string Description { get; set; }
}

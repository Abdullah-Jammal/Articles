using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Users;

public class UserRole : IdentityUserRole<int>
{
    public DateTime? StartDate { get; set; }
    public DateTime? ExpiringDate { get; set; }
}

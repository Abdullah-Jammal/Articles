using Mapster;

namespace Auth.Domain.Users;

public partial class UserRole
{
    public static UserRole Create(IUserRole userRoleInfo)
    {
        if (userRoleInfo.StartDate.HasValue && userRoleInfo.ExpiringDate.HasValue && userRoleInfo.StartDate > userRoleInfo.ExpiringDate)
        {
            throw new ArgumentException("Start date cannot be later than expiring date.");
        }

        var userRole = userRoleInfo.Adapt<UserRole>();
        return userRole;
    }
}

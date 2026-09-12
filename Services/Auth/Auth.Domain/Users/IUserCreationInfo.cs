using Articles.Abstractions.Enums;

namespace Auth.Domain.Users;

public interface IUserCreationInfo
{
    string? Affiliation { get; }
    string? CompanyName { get; }
    string Email { get; }
    string FirstName { get; }
    Gender Gender { get; }
    Honorific? Honorific { get; }
    string LastName { get; }
    string? PhoneNumber { get; }
    string? PictureUrl { get; }
    string? Position { get; }
    IReadOnlyList<IUserRole> UserRoles { get; }
}

public interface IUserRole
{
    DateTime? ExpiringDate { get; }
    DateTime? StartDate { get; }
    UserRoleType Type { get; }
}

using Auth.Domain.Users.ValueObjects;
using Blocks.Core.Extensions;

namespace Auth.Domain.Users;

public partial class User
{
    public static User Create(IUserCreationInfo userInfo)
    {
        if (userInfo.UserRoles.IsNullOrEmpty())
        {
            throw new ArgumentException("User must have at least one role assigned.", nameof(userInfo.UserRoles));
        }

        var user = new User
        {
            UserName = userInfo.Email,
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Gender = userInfo.Gender,
            PhoneNumber = userInfo.PhoneNumber,
            ProfessionalProfile = ProfessionalProfile.Create(userInfo.Affiliation, userInfo.CompanyName, userInfo.Position),
            PictureUrl = userInfo.PictureUrl,
            Honorific = HonorificTile.Create(userInfo.Honorific),
            _userRoles = userInfo.UserRoles.Select(r => UserRole.Create(r)).ToList()
        };

        return user;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        _refreshTokens.Add(refreshToken);
    }
}

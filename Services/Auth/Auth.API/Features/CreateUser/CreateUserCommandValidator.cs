using FluentValidation;
using FastEndpoints;

namespace Auth.API.Features.CreateUser;

public class CreateUserCommandValidator : Validator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(c => c.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(c => c.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");

        RuleFor(c => c.UserRoles).NotEmpty()
            .Must((c, roles) => AreUserRoleDatesValid(roles))
            .WithMessage("Invalid user role dates.");
    }

    public static bool AreUserRoleDatesValid(IReadOnlyList<UserRoleDto> roles)
    {
        return roles.All(role => 
        (!role.StartDate.HasValue || role.StartDate.Value >= DateTime.UtcNow.Date) &&
        (!role.ExpiringDate.HasValue || role.ExpiringDate.Value > DateTime.UtcNow) &&
        (!role.StartDate.HasValue || !role.ExpiringDate.HasValue || role.StartDate.Value < role.ExpiringDate.Value)
        );
    }
}

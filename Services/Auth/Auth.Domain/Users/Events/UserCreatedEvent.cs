namespace Auth.Domain.Users.Events;

public record UserCreatedEvent(User user, string ResetPasswordToken);
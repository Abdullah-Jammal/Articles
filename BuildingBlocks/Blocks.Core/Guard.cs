using Blocks.Exceptions;

namespace Blocks.Core;

public static class Guard
{
    public static void ThrowIfNullOrEmpty(string value)
     => ArgumentException.ThrowIfNullOrEmpty(value);
    public static void ThrowIfNullOrWhiteSpace(string value)
    => ArgumentException.ThrowIfNullOrWhiteSpace(value);

    public static void ThrowIfFalse(this bool condtion, string message = "Condition must be true.")
    {
        if (!condtion) throw new ArgumentException(message);
    }

    public static T NotFount<T>(T? value) where T : class =>
        value ?? throw new NotFoundException($"{typeof(T).Name} not found");
}

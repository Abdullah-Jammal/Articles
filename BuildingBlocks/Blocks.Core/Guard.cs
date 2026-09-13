namespace Blocks.Core;

public static class Guard
{
    public static void ThrowIfNullOrEmpty(string value)
     => ArgumentException.ThrowIfNullOrEmpty(value);
    public static void ThrowIfNullOrWhiteSpace(string value)
    => ArgumentException.ThrowIfNullOrWhiteSpace(value);
}
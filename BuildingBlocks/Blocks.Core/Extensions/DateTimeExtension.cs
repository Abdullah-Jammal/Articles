namespace Blocks.Core;

public static class DateTimeExtension
{
    public static long ToUnixEpochDate(this DateTime date)
    {
        return (long)Math.Round((date.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);
    }
}
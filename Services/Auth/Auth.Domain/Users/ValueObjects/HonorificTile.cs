using Auth.Domain.Users.Enums;
using Blocks.Core;
using Blocks.Domain.ValueObjects;

namespace Auth.Domain.Users.ValueObjects;

public class HonorificTile : StringValueObject
{
    private HonorificTile(string value) => Value = value;

    public static HonorificTile Create(string honorific)
    {
        Guard.ThrowIfNullOrWhiteSpace(honorific);
        return new HonorificTile(honorific.Trim());
    }
    public static HonorificTile Create(Honorific? honorific)
    {
        if (honorific is null)
        {
            return new HonorificTile(string.Empty);
        }
        return new HonorificTile(honorific.Value.ToString());
    }
}

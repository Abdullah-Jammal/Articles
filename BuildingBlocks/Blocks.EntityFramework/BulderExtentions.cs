using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blocks.EntityFramework;

public static class BulderExtentions
{
    public static PropertyBuilder<TEnum> HasEnumConversion<TEnum>(this PropertyBuilder<TEnum> builder) 
        where TEnum : Enum
    {
        return builder.HasConversion(
            v => v.ToString(),
            v => (TEnum)Enum.Parse(typeof(TEnum), v)
        );
    }
}

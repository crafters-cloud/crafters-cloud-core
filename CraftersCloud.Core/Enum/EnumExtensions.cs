using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using JetBrains.Annotations;

namespace CraftersCloud.Core.Enum;

[PublicAPI]
public static class EnumExtensions
{
    public static string GetDisplayName(this System.Enum value)
    {
        var enumType = value.GetType();

        var memberInfo = enumType.GetMember(value.ToString());
        var attribute = memberInfo
            .First()
            .GetCustomAttribute(typeof(DisplayAttribute));

        return attribute is DisplayAttribute attr ? attr.Name ?? string.Empty : string.Empty;
    }

    public static string GetCode<TEnum>(this TEnum o) => o.FindAttribute<TEnum, CodeAndDescriptionAttribute>()!.Code;

    public static string? FindCode<TEnum>(this TEnum o) => o.FindAttribute<TEnum, CodeAndDescriptionAttribute>()?.Code;

    public static string GetDescription<TEnum>(this TEnum o) =>
        o.GetAttribute<TEnum, DescriptionAttribute>().Description;

    public static TDescriptionAttribute GetAttribute<TEnum, TDescriptionAttribute>(this TEnum o)
        where TDescriptionAttribute : DescriptionAttribute
    {
        var result = FindAttribute<TEnum, TDescriptionAttribute>(o);
        var attributeType = typeof(TDescriptionAttribute);
        return result ?? throw new InvalidOperationException($"Attribute of type {attributeType} was not found");
    }

    private static TDescriptionAttribute? FindAttribute<TEnum, TDescriptionAttribute>(this TEnum o)
        where TDescriptionAttribute : DescriptionAttribute
    {
        var enumType = o!.GetType();
        var field = enumType.GetField(o.ToString() ?? string.Empty);
        var attributeType = typeof(TDescriptionAttribute);
        var attributes = field != null ? field.GetCustomAttributes(attributeType, false) : Array.Empty<object>();
        return attributes.Length == 0 ? null : (TDescriptionAttribute) attributes[0];
    }
}
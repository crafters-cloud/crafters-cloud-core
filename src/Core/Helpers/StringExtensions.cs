using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using static System.Char;
using static System.String;

namespace CraftersCloud.Core.Helpers;

public static class StringExtensions
{
    private const string EmailRegex =
        @"^ *[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](.?[-!#$%&'*+/0-9=?A-Z^_a-z`{|}~])*@[a-zA-Z0-9](-*.?[a-zA-Z0-9])+((\.(\w){2,})+) *$";

    //return true if the string is null or empty


    public static bool HasContent([NotNullWhen(true)] this string? value) => !IsNullOrEmpty(value);

    public static string ToEmptyIfNull(this string? value) => value ?? Empty;

    public static string ToCamelCase(this string s)
    {
        if (IsNullOrEmpty(s) || !IsUpper(s[0]))
        {
            return s;
        }

        var chars = s.ToCharArray();

        for (var i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !IsUpper(chars[i]))
            {
                break;
            }

            var hasNext = i + 1 < chars.Length;
            if (i > 0 && hasNext && !IsUpper(chars[i + 1]))
            {
                break;
            }

            chars[i] = ToLower(chars[i], CultureInfo.InvariantCulture);
        }

        return new string(chars);
    }

    public static string JoinString(this IEnumerable<string> values, string separator) => Join(separator, values);

    public static string JoinString(this IEnumerable<int> values, string separator) => Join(separator, values);

    public static string JoinString<TEnum>(this IEnumerable<TEnum> values, string separator) => Join(separator, values);

    public static bool IsValidEmailAddress(this string source) =>
        source.HasContent() && Regex.IsMatch(source, EmailRegex);
}
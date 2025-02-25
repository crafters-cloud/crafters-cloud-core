using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;
using static System.Char;
using static System.String;

namespace CraftersCloud.Core.Helpers;

[PublicAPI]
public static partial class StringExtensions
{
    private const string EmailRegex =
        @"^ *[-!#$%&'*+/0-9=?A-Z^_a-z{|}~](.?[-!#$%&'*+/0-9=?A-Z^_a-z`{|}~])*@[a-zA-Z0-9](-*.?[a-zA-Z0-9])+((\.(\w){2,})+) *$";

    /// <summary>
    /// Determines whether the specified string has content, i.e., it is not null or empty.
    /// </summary>
    /// <param name="value">The nullable string to check.</param>
    /// <return>True if the string is not null or empty; otherwise, false.</return>
    public static bool HasContent([NotNullWhen(true)] this string? value) => !IsNullOrEmpty(value);

    /// <summary>
    /// Returns an empty string if the input string is null, otherwise returns the input string itself.
    /// </summary>
    /// <param name="value">The nullable string to evaluate.</param>
    /// <return>A non-null string. Returns the original string if it is not null; otherwise, an empty string.</return>
    public static string ToEmptyIfNull(this string? value) => value ?? Empty;


    /// <summary>
    /// Removes all characters after the specified character in a string, including the character itself, if present.
    /// If the specified character is not found, the original string is returned.
    /// If the string is null or empty, an empty string is returned.
    /// </summary>
    /// <param name="value">The input string to process.</param>
    /// <param name="character">The character after which content will be removed.</param>
    /// <return>
    /// The resultant string after removing all characters from the first occurrence of the specified character.
    /// If the character is not found, the original string is returned.
    /// </return>
    public static string RemoveAfter(this string? value, char character)
    {
        if (IsNullOrEmpty(value))
        {
            return Empty;
        }
    
        var index = value.IndexOf(character);
        return index == -1 ? value : value[..index];
    }

    /// <summary>
    /// Converts the specified string from PascalCase to camelCase if its first character is uppercase.
    /// If the first character is not uppercase, the input string remains unchanged.
    /// </summary>
    /// <param name="s">The string to convert to camelCase.</param>
    /// <return>The camelCased string if the input starts with an uppercase character; otherwise, the original string.</return>
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

    /// <summary>
    /// Concatenates the members of a collection of strings, using the specified separator between each member.
    /// </summary>
    /// <param name="values">The collection of strings to concatenate.</param>
    /// <param name="separator">The string to use as a separator. This separator is included in the returned string only if the collection has more than one element.</param>
    /// <return>A string that consists of the elements of the collection, delimited by the separator string. Returns an empty string if the collection is empty.</return>
    public static string JoinStrings(this IEnumerable<string> values, string separator) => Join(separator, values);

    /// <summary>
    /// Concatenates the string representations of a collection of elements of type T, using the specified separator between each element.
    /// </summary>
    /// <param name="values">The collection of elements of type T to concatenate.</param>
    /// <param name="valueToString">A function that converts each element to its string representation.</param>
    /// <param name="separator">The string to use as a separator. This separator is included in the returned string only if the collection has more than one element.</param>
    /// <return>A string that consists of the string representations of the elements of the collection, delimited by the separator string. Returns an empty string if the collection is empty.</return>
    public static string JoinStrings<T>(this IEnumerable<T> values, Func<T, string> valueToString, string separator) => values.Select(valueToString).JoinStrings(separator);

    /// <summary>
    /// Determines whether the specified string is a valid email address based on a predefined regex pattern.
    /// </summary>
    /// <param name="source">The string to validate as an email address.</param>
    /// <return>True if the string is a valid email address; otherwise, false.</return>
    public static bool IsValidEmailAddress(this string source) =>
        source.HasContent() && MyRegex().IsMatch(source);

    [GeneratedRegex(EmailRegex)]
    private static partial Regex MyRegex();
}
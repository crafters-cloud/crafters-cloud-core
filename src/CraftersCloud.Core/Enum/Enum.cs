namespace CraftersCloud.Core.Enum;

public static class Enum<TEnum> where TEnum : struct, IComparable, IFormattable, IConvertible
{
    public static TEnum GetMatchingEnum(string match)
    {
        var result = GetAll()
            .FirstOrDefault(x => Convert.ToInt32(x).ToString() == match ||
                                 x.ToString() == match
                                 || x.FindCode() == match);
        return result;
    }

    public static IList<TEnum> GetAll()
    {
        var t = typeof(TEnum);
        return !t.IsEnum
            ? new List<TEnum>()
            : System.Enum.GetValues(t).Cast<TEnum>().ToList();
    }
}
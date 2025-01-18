namespace batteries.Extensions;

public static class StringExtensions
{
    public static string TrimAfter(this string str, char separator)
    {
        var index = str.IndexOf(separator);
        return index >= 0 ? str.Substring(0, index) : str;
    }
}
namespace PdfLibCore.Parser.Helpers;

public static class NameHelper
{
    public static string GetName<T>()
        where T : Attribute
    {
        return typeof(T).Name.Replace(nameof(Attribute), string.Empty);
    }
    
    public static string ToCSharp(string filename)
    {
        var toUpper = false;
        if (filename.StartsWith("_"))
        {
            filename = filename[1..];
            toUpper = true;
        }
        if (filename.EndsWith("_t__"))
        {
            filename = filename[..^4];
            toUpper = true;
        }
        if (filename.EndsWith("_"))
        {
            filename = filename[..^1];
            toUpper = true;
        }
        return toUpper ? filename.ToUpper() : filename;
    }
}
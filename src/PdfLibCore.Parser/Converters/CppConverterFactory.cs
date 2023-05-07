using CppAst;

namespace PdfLibCore.Parser.Converters;

public static class CppConverterFactory
{
    public static ICppConverter GetCppConverter<T>(T cppElement)
        where T : ICppElement
    {
        return cppElement switch
        {
            CppEnum cpp => new CppEnumConverter(cpp),
            CppFunction cpp => new CppFunctionConverter(cpp),
            CppClass cpp => new CppClassConverter(cpp),
            CppTypedef cpp => new CppTypedefConverter(cpp),
            _ => throw new ArgumentException()
        };
    }
}
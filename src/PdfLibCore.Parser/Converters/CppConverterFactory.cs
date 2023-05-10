using CppAst;
using Serilog;

namespace PdfLibCore.Parser.Converters;

public static class CppConverterFactory
{
    public static ICppConverter? GetCppConverter(SourceElement? sourceElement, ILogger logger)
    {
        return sourceElement?.Element switch
        {
            CppEnum cpp => new CppEnumConverter(cpp, logger),
            CppClass cpp => new CppClassConverter(cpp, logger),
            CppTypedef cpp => CppTypedefConverter.CanBeConverted(cpp) ? new CppTypedefConverter(cpp, logger) : null,
            _ => null
        };
    }
}
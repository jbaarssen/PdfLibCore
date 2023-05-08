// ReSharper disable once CheckNamespace

namespace CppAst;

public static class CppClassExtensions
{
    public static bool HasComments<T>(this T cppClass) where T : ICppDeclaration =>
        cppClass.Comment != null;
}
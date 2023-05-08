
// ReSharper disable once CheckNamespace
namespace CppAst;

public static class CppClassExtensions
{
    public static bool HasComments(this CppClass cppClass) =>
        cppClass.Comment != null;
}
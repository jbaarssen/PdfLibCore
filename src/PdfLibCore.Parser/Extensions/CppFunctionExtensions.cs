using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;

// ReSharper disable once CheckNamespace
namespace CppAst;

public static class CppFunctionExtensions
{
    public static ParameterSyntax[] GetParameters(this CppFunction function, Func<CppParameter, TypeSyntax?>? typeFunc = null)
    {
        return function.Parameters
            .Select(parameter => SyntaxFactory.Parameter(SyntaxFactory.Identifier(NameHelper.ToCSharp(parameter.Name)))
                .WithType(typeFunc != null
                    ? typeFunc(parameter) ?? parameter.Type.ToCSharp()
                    : parameter.Type.ToCSharp()))
            .ToArray();
    }
}
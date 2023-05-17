using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

// ReSharper disable once CheckNamespace
namespace CppAst;

public static class CppFunctionExtensions
{
    public static ParameterSyntax[] GetParameters(this CppFunction function, Func<CppParameter, TypeSyntax?>? typeFunc = null)
    {
        var parameters = new List<ParameterSyntax>();
        foreach (var cpp in function.Parameters)
        {
            var p = Parameter(Identifier(NameHelper.ToCSharp(cpp.Name)))
                .WithType(typeFunc != null
                    ? typeFunc(cpp) ?? cpp.Type.ToCSharp()
                    : cpp.Type.ToCSharp());

            if (cpp.Type is CppPointerType pointerType)
            {
                p = p.AddModifiers(Token(pointerType.ElementType.TypeKind == CppTypeKind.Primitive ? SyntaxKind.OutKeyword : SyntaxKind.RefKeyword));
            }
            parameters.Add(p);
        }
        return parameters.ToArray();
    }
}
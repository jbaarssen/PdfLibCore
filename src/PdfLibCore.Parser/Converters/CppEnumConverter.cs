﻿using System.Collections.Generic;
using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed class CppEnumConverter : BaseCppConverter<CppEnum>
{
    public CppEnumConverter(CppEnum cppEnum)
        : base(cppEnum)
    {
    }

    public override CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit)
    {
        var list = new List<SyntaxNodeOrToken>();
        foreach (var item in CppElement.Items)
        {
            list.Add(
                EnumMemberDeclaration(Identifier(item.Name))
                    .WithEqualsValue(EqualsValueClause(
                        ToExpressionSyntax(item.ValueExpression) ?? LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(item.Value)))));
            if (CppElement.Items.Last() != item)
            {
                list.Add(Token(SyntaxKind.CommaToken));
            }
        }

        return compilationUnit
            .AddMembers(
                EnumDeclaration(CppElement.Name)
                    .AddModifiers(CppElement.Visibility.ToCSharp())
                    .WithMembers(SeparatedList<EnumMemberDeclarationSyntax>(list)));
    }
}
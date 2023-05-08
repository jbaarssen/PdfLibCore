using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed class CppEnumConverter : BaseCppConverter<CppEnum>
{
    public CppEnumConverter(CppEnum cppEnum)
        : base(cppEnum.Name, cppEnum)
    {
    }

    public override CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit)
    {
        return compilationUnit
            .AddMembers(
                EnumDeclaration(ElementName)
                    .AddModifiers(CppElement.Visibility.ToCSharp())
                    .AddMembers(CppElement.Items
                        .Select(item =>
                            EnumMemberDeclaration(Identifier(item.Name))
                                .WithEqualsValue(EqualsValueClause(ToExpressionSyntax(item.ValueExpression)
                                    ?? LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal((int)item.Value))))).ToArray()));
    }
}
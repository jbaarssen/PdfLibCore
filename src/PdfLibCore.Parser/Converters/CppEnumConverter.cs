using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed class CppEnumConverter : BaseCppConverter<CppEnum>
{
    public CppEnumConverter(CppEnum cppEnum, ILogger logger)
        : base(cppEnum, logger)
    {
    }

    protected override CompilationUnitSyntax OnConvert(CompilationUnitSyntax compilationUnit)
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
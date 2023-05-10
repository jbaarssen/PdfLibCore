using CppAst;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed partial class CppConverter
{
    private CompiledSource CreateIHandleInterface()
    {
        var sourceElement = new SourceElement("IHandle", "Interfaces");
        _logger.Information("Creating '{File}' interface", sourceElement.Name);
        var unit = GetCompilationUnit(sourceElement)
            .AddMembers(
                InterfaceDeclaration(sourceElement.Name)
                    .AddModifiers(CppVisibility.Public.ToCSharp())
                    .AddTypeParameterListParameters(TypeParameter(Identifier("T")).WithVarianceKeyword(Token(SyntaxKind.OutKeyword)))
                    .AddMembers(
                        PropertyDeclaration(
                                PredefinedType(Token(SyntaxKind.BoolKeyword)),
                                Identifier("IsNull"))
                            .AddAccessorListAccessors(
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))),
                        MethodDeclaration(
                                IdentifierName("T"),
                                Identifier("SetToNull"))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                    ));

        return new CompiledSource(sourceElement, unit);
    }
}
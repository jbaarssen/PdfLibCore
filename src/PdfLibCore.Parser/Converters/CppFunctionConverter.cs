using System.Runtime.InteropServices;
using System.Security;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Converters.Builders;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed class CppFunctionConverter : BaseCppConverter<CppFunction>
{
    public CppFunctionConverter(CppFunction cppElement)
        : base(cppElement)
    {
    }

    public override CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit)
    {
        var commentBuilder = new MethodCommentBuilder(CppElement);
        var parameterBuilder = new MethodParameterBuilder(CppElement);

        return compilationUnit.AddMembers(MethodDeclaration(
                PredefinedType(Token(SyntaxKind.BoolKeyword)),
                Identifier(CppElement.Name))
            .WithAttributeLists(GetAttributes())
            .WithLeadingTrivia(commentBuilder.GetComments())
            .WithModifiers(TokenList(
                Token(SyntaxKind.InternalKeyword),
                Token(SyntaxKind.StaticKeyword),
                Token(SyntaxKind.ExternKeyword)))
            .WithParameterList(parameterBuilder.GetParameters())
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
    }

    private SyntaxList<AttributeListSyntax> GetAttributes()
    {
        return SingletonList(AttributeList().AddAttributes(
            Attribute(IdentifierName(NameHelper.GetName<DllImportAttribute>()))
                .AddArgumentListArguments(
                    AttributeArgument(IdentifierName("DllName")),
                    AttributeArgument(
                            InvocationExpression(IdentifierName(Identifier(TriviaList(), SyntaxKind.NameOfKeyword, "nameof", "nameof", TriviaList())))
                                .AddArgumentListArguments(Argument(IdentifierName(CppElement.Name))))
                        .WithNameEquals(NameEquals(IdentifierName("EntryPoint"))),
                    AttributeArgument(LiteralExpression(SyntaxKind.TrueLiteralExpression))
                        .WithNameEquals(NameEquals(IdentifierName("SetLastError")))),
            Attribute(IdentifierName(NameHelper.GetName<SuppressUnmanagedCodeSecurityAttribute>()))));
    }
}
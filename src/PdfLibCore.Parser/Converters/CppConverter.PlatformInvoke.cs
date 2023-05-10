using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed partial class CppConverter
{
    private const string DllName = "DllName";

    private CompiledSource CreatePlatformInvoke()
    {
        // For now we are filtering out any method that has an input parameter or return type (like a function) that is not currently (or easily) supported
        var functions = _compilation.Functions
            .Where(f => f.ReturnType.ToCSharp() != null && f.Parameters.All(p => p.Type.ToCSharp() != null))
            .Select(Create)
            .ToArray();

        var sourceElement = new SourceElement("PlatformInvoke");
        var unit = GetCompilationUnit()
            .AddUsings(SyntaxHelper.AddUsingDirectives(
                "System",
                "System.Security",
                "System.Runtime.InteropServices",
                "PdfLibCore.Generated.Enums",
                "PdfLibCore.Generated.Structs",
                "PdfLibCore.Generated.Types"))
            .AddMembers(
                ClassDeclaration(sourceElement.Name)
                    .AddModifiers(Token(SyntaxKind.InternalKeyword), Token(SyntaxKind.StaticKeyword))
                    .AddMembers(
                        FieldDeclaration(VariableDeclaration(
                                PredefinedType(Token(SyntaxKind.StringKeyword)),
                                SeparatedList(new[]
                                {
                                    VariableDeclarator(Identifier(DllName))
                                        .WithInitializer(
                                            EqualsValueClause(
                                                LiteralExpression(
                                                    SyntaxKind.StringLiteralExpression,
                                                    Literal("pdfium"))))
                                })
                            )
                        ).AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ConstKeyword))
                    )
                    .AddMembers(functions)
            )
            .NormalizeWhitespace();

        return new CompiledSource(sourceElement, unit);
    }

    private MemberDeclarationSyntax Create(CppFunction function)
    {
        return MethodDeclaration(
                function.ReturnType.ToCSharp()!,
                Identifier(function.Name))
            .AddAttributeLists(GetAttributes(function))
            .WithLeadingTrivia(function.GetComments())
            .AddModifiers(
                Token(SyntaxKind.InternalKeyword),
                Token(SyntaxKind.StaticKeyword),
                Token(SyntaxKind.ExternKeyword))
            .AddParameterListParameters(function.GetParameters(cppParameter =>
                "HDC".Equals(cppParameter.Type.GetDisplayName(), StringComparison.InvariantCultureIgnoreCase)
                    ? IdentifierName("FPDF_BITMAP")
                    : cppParameter.Type.ToCSharp()))
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static AttributeListSyntax[] GetAttributes(ICppMember function)
    {
        return new[]
        {
            AttributeList()
                .AddAttributes(
                    Attribute(NameHelper.GetName<DllImportAttribute>())
                        .AddArgumentListArguments(
                            AttributeArgument(IdentifierName(DllName)),
                            AttributeArgument(
                                    InvocationExpression(IdentifierName(Identifier(TriviaList(), SyntaxKind.NameOfKeyword, "nameof", string.Empty, TriviaList())))
                                        .AddArgumentListArguments(Argument(IdentifierName(function.Name))))
                                .WithNameEquals(NameEquals(IdentifierName(nameof(DllImportAttribute.EntryPoint)))),
                            AttributeArgument(LiteralExpression(SyntaxKind.TrueLiteralExpression))
                                .WithNameEquals(NameEquals(IdentifierName(nameof(DllImportAttribute.SetLastError))))),
                    Attribute(NameHelper.GetName<SuppressUnmanagedCodeSecurityAttribute>()))
        };
    }
}
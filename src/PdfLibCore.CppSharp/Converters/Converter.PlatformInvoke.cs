using System;
using System.Collections.Generic;
using System.IO;
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

public sealed partial class Converter
{
    private const string DllName = "DllName";

    private IEnumerable<CompiledSource> CreatePlatformInvoke()
    {
        yield return CreateFile("PlatformInvoke", () => new MemberDeclarationSyntax[]
        {
            FieldDeclaration(
                VariableDeclaration(PredefinedType(Token(SyntaxKind.StringKeyword)))
                    .AddVariables(VariableDeclarator(
                        Identifier(DllName),
                        null,
                        EqualsValueClause(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("pdfium")))))
            ).AddModifiers(Token(SyntaxKind.PrivateKeyword), Token(SyntaxKind.ConstKeyword))
        });

        // For now we are filtering out any method that has an input parameter or return type (like a function) that is not currently (or easily) supported
        // These methods can be added 'manually' since the PlatformInvoke classes are parial.
        var groups = _compilation.Functions
            .Where(f => f.ReturnType.ToCSharp() != null && f.Parameters.All(p => p.Type.ToCSharp() != null))
            .GroupBy(f => f.SourceFile)
            .ToDictionary(f => ToPartialFileName(f.Key), f => f.ToList());

        foreach (var group in groups)
        {
            yield return CreateFile(
                $"PlatformInvoke.{Path.GetFileNameWithoutExtension(group.Key)}",
                () => group.Value.Select(Create).ToArray(),
                () => SyntaxHelper.AddUsingDirectives(
                    "System",
                    "System.Security",
                    "System.Runtime.InteropServices",
                    "PdfLibCore.Generated.Enums",
                    "PdfLibCore.Generated.Structs",
                    "PdfLibCore.Generated.Types"));
        }
    }

    private CompiledSource CreateFile(string name, Func<MemberDeclarationSyntax[]> funcMethods, Func<UsingDirectiveSyntax[]>? funcUsings = null)
    {
        var sourceElement = new SourceElement(name);
        var unit = funcUsings == null ? GetCompilationUnit() : GetCompilationUnit().AddUsings(funcUsings!());
        return new CompiledSource(sourceElement, unit.AddMembers(
            ClassDeclaration(sourceElement.Name.Split('.')[0])
                .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword), Token(SyntaxKind.PartialKeyword))
                .AddMembers(funcMethods())));
    }

    private static string ToPartialFileName(string filename)
    {
        var name = Path.GetFileNameWithoutExtension(filename).Replace(".h", string.Empty).Replace("fpdf_", string.Empty);
        return $"{char.ToUpper(name[0])}{name[1..]}";
    }

    private static MemberDeclarationSyntax Create(CppFunction function)
    {
        return MethodDeclaration(
                function.ReturnType.ToCSharp()!,
                Identifier(function.Name))
            .AddAttributeLists(GetAttributes(function))
            .WithLeadingTrivia(function.GetComments())
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
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
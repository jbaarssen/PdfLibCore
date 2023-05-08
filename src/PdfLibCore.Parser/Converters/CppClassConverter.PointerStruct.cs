using System;
using System.Threading;
using CppAst;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public partial class CppClassConverter
{
    private const string FieldName = "_pointer";

    private CompilationUnitSyntax CreatePointerStruct(CompilationUnitSyntax compilationUnitSyntax)
    {
        return compilationUnitSyntax.AddUsings(UsingDirective(
                    IdentifierName("System")),
                UsingDirective(
                    QualifiedName(
                        IdentifierName("System"),
                        IdentifierName("Threading"))))
            .AddMembers(
                StructDeclaration(NameHelper.ToCSharp(CppElement.Name))
                    .AddModifiers(CppElement.Visibility.ToCSharp())
                    .AddBaseListTypes(
                        SimpleBaseType(
                            GenericName(Identifier("IHandle"))
                                .AddTypeArgumentListArguments(IdentifierName(NameHelper.ToCSharp(CppElement.Name)))))
                    .AddMembers(
                        CreateField(nameof(IntPtr), FieldName, CppVisibility.Private),
                        CreateProperty("bool", "IsNull", CppVisibility.Public, () => ArrowExpressionClause(
                            BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                IdentifierName(FieldName),
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName(nameof(IntPtr)),
                                    IdentifierName(nameof(IntPtr.Zero)))))),
                        CreateProperty(NameHelper.ToCSharp(CppElement.Name), "Null", CppVisibility.Public, () =>
                                ArrowExpressionClause(ImplicitObjectCreationExpression()))
                            .AddModifiers(Token(SyntaxKind.StaticKeyword)),

                        //CreateConstructor(),
                        ConstructorDeclaration(
                                Identifier(NameHelper.ToCSharp(CppElement.Name)))
                            .AddModifiers(CppVisibility.Private.ToCSharp())
                            .AddParameterListParameters(Parameter(Identifier("ptr")).WithType(IdentifierName(nameof(IntPtr))))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName(FieldName),
                                        IdentifierName("ptr"))))
                            .WithSemicolonToken(
                                Token(SyntaxKind.SemicolonToken)),
                        MethodDeclaration(
                                IdentifierName(NameHelper.ToCSharp(CppElement.Name)),
                                Identifier("SetToNull"))
                            .WithExplicitInterfaceSpecifier(
                                ExplicitInterfaceSpecifier(
                                    GenericName(Identifier("IHandle"))
                                        .AddTypeArgumentListArguments(IdentifierName(NameHelper.ToCSharp(CppElement.Name)))))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    ImplicitObjectCreationExpression()
                                        .AddArgumentListArguments(
                                            Argument(
                                                InvocationExpression(
                                                        MemberAccessExpression(
                                                            SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName(nameof(Interlocked)),
                                                            IdentifierName(nameof(Interlocked.Exchange))))
                                                    .AddArgumentListArguments(
                                                        Argument(
                                                                IdentifierName(FieldName))
                                                            .WithRefOrOutKeyword(
                                                                Token(SyntaxKind.RefKeyword)),
                                                        Argument(
                                                            MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                IdentifierName(nameof(IntPtr)),
                                                                IdentifierName(nameof(IntPtr.Zero))))
                                                    )))))
                            .WithSemicolonToken(
                                Token(SyntaxKind.SemicolonToken)),
                        MethodDeclaration(
                                CppPrimitiveType.WChar.ToCSharp(),
                                Identifier(nameof(ToString)))
                            .AddModifiers(CppVisibility.Public.ToCSharp(), Token(SyntaxKind.OverrideKeyword))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    InterpolatedStringExpression(
                                            Token(SyntaxKind.InterpolatedStringStartToken))
                                        .AddContents(
                                            InterpolatedStringText()
                                                .WithTextToken(
                                                    Token(
                                                        TriviaList(),
                                                        SyntaxKind.InterpolatedStringTextToken,
                                                        "FPDF_ANNOTATION: 0x",
                                                        string.Empty,
                                                        TriviaList())),
                                            Interpolation(IdentifierName("_pointer"))
                                                .WithFormatClause(
                                                    InterpolationFormatClause(Token(SyntaxKind.ColonToken))
                                                        .WithFormatStringToken(
                                                            Token(
                                                                TriviaList(),
                                                                SyntaxKind.InterpolatedStringTextToken,
                                                                "X16",
                                                                string.Empty,
                                                                TriviaList())))
                                        )))
                            .WithSemicolonToken(
                                Token(SyntaxKind.SemicolonToken))
                    ));
    }
}
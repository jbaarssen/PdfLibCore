using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public partial class CppClassConverter
{
    private const string FieldName = "_pointer";

    private static FieldDeclarationSyntax CreateField(string type, string variable, CppVisibility visibility)
    {
        return FieldDeclaration(VariableDeclaration(IdentifierName(type))
                .AddVariables(VariableDeclarator(Identifier(variable))))
            .WithModifiers(visibility.ToCSharp());
    }

    private static PropertyDeclarationSyntax CreateProperty(string type, string name, CppVisibility visibility, Func<ArrowExpressionClauseSyntax> func)
    {
        return PropertyDeclaration(
                IdentifierName(type),
                Identifier(name))
            .WithModifiers(visibility.ToCSharp())
            .WithExpressionBody(func())
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private TypeDeclarationSyntax CreatePointerStruct()
    {
        return StructDeclaration(NameHelper.ToCSharp(CppElement.Name))
            .WithModifiers(CppElement.Visibility.ToCSharp())
            .WithBaseList(
                BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(
                            GenericName(Identifier("IHandle"))
                                .WithTypeArgumentList(
                                    TypeArgumentList(
                                        SingletonSeparatedList<TypeSyntax>(
                                            IdentifierName(NameHelper.ToCSharp(CppElement.Name)))))))))
            .WithMembers(
                List(
                    new MemberDeclarationSyntax[]
                    {
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

                        ConstructorDeclaration(
                                Identifier(NameHelper.ToCSharp(CppElement.Name)))
                            .WithModifiers(CppVisibility.Private.ToCSharp())
                            .WithParameterList(
                                ParameterList(
                                    SingletonSeparatedList(
                                        Parameter(
                                                Identifier("ptr"))
                                            .WithType(
                                                IdentifierName(nameof(IntPtr))))))
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
                                    GenericName(
                                            Identifier("IHandle"))
                                        .WithTypeArgumentList(
                                            TypeArgumentList(
                                                SingletonSeparatedList<TypeSyntax>(
                                                    IdentifierName(NameHelper.ToCSharp(CppElement.Name)))))))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    ImplicitObjectCreationExpression()
                                        .WithArgumentList(
                                            ArgumentList(
                                                SingletonSeparatedList(
                                                    Argument(
                                                        InvocationExpression(
                                                                MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    IdentifierName(nameof(Interlocked)),
                                                                    IdentifierName(nameof(Interlocked.Exchange))))
                                                            .WithArgumentList(
                                                                ArgumentList(
                                                                    SeparatedList<ArgumentSyntax>(
                                                                        new SyntaxNodeOrToken[]
                                                                        {
                                                                            Argument(
                                                                                    IdentifierName(FieldName))
                                                                                .WithRefOrOutKeyword(
                                                                                    Token(SyntaxKind.RefKeyword)),
                                                                            Token(SyntaxKind.CommaToken),
                                                                            Argument(
                                                                                MemberAccessExpression(
                                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                                    IdentifierName(nameof(IntPtr)),
                                                                                    IdentifierName(nameof(IntPtr.Zero))))
                                                                        })))))))))
                            .WithSemicolonToken(
                                Token(SyntaxKind.SemicolonToken)),

                        MethodDeclaration(
                                CppPrimitiveType.Char.ToCSharp(),
                                Identifier(nameof(ToString)))
                            .WithModifiers(
                                TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.OverrideKeyword)))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    InterpolatedStringExpression(
                                            Token(SyntaxKind.InterpolatedStringStartToken))
                                        .WithContents(
                                            List(
                                                new InterpolatedStringContentSyntax[]
                                                {
                                                    InterpolatedStringText()
                                                        .WithTextToken(
                                                            Token(
                                                                TriviaList(),
                                                                SyntaxKind.InterpolatedStringTextToken,
                                                                "FPDF_ANNOTATION: 0x",
                                                                "FPDF_ANNOTATION: 0x",
                                                                TriviaList())),
                                                    Interpolation(
                                                        InvocationExpression(
                                                                MemberAccessExpression(
                                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                                    IdentifierName(FieldName),
                                                                    IdentifierName(nameof(FieldName.ToString))))
                                                            .WithArgumentList(
                                                                ArgumentList(
                                                                    SingletonSeparatedList(
                                                                        Argument(
                                                                            LiteralExpression(
                                                                                SyntaxKind.StringLiteralExpression,
                                                                                Literal("X16")))))))
                                                }))))
                            .WithSemicolonToken(
                                Token(SyntaxKind.SemicolonToken))
                    }));
    }
}
using System;
using System.Threading;
using CppAst;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using Serilog;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed class CppTypedefConverter : BaseCppConverter<CppTypedef>
{

    private const string FieldName = "_pointer";
    private const string VariableName = "ptr";

    public static bool CanBeConverted(CppTypedef cppElement)
    {
        return cppElement.ElementType switch
        {
            CppPointerType p when p.ElementType.TypeKind == CppTypeKind.Primitive => true,
            CppPointerType p when p.ElementType.TypeKind == CppTypeKind.StructOrClass => true,
            CppPointerType p when p.ElementType.TypeKind == CppTypeKind.Qualified => true,
            CppQualifiedType => true,
            _ => false
        };
    }

    public CppTypedefConverter(CppTypedef cppElement, ILogger logger)
        : base(cppElement, logger)
    {
    }

    protected override CompilationUnitSyntax OnConvert(CompilationUnitSyntax compilationUnit)
    {
        return compilationUnit.AddMembers()
            .AddUsings(SyntaxHelper.AddUsingDirectives("System", "System.Threading", "PdfLibCore.Generated.Interfaces"))
            .AddMembers(
                StructDeclaration(ElementName)
                    .AddModifiers(CppElement.Visibility.ToCSharp())
                    .AddBaseListTypes(
                        SimpleBaseType(
                            GenericName(Identifier("IHandle"))
                                .AddTypeArgumentListArguments(IdentifierName(ElementName))))
                    .AddMembers(
                        SyntaxHelper.CreateField(nameof(IntPtr), FieldName, CppVisibility.Private),
                        SyntaxHelper.CreateProperty("bool", "IsNull", CppVisibility.Public, () => ArrowExpressionClause(
                            BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                IdentifierName(FieldName),
                                MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    IdentifierName(nameof(IntPtr)),
                                    IdentifierName(nameof(IntPtr.Zero)))))),
                        SyntaxHelper.CreateProperty(ElementName, "Null", CppVisibility.Public, () =>
                                ArrowExpressionClause(ImplicitObjectCreationExpression()))
                            .AddModifiers(Token(SyntaxKind.StaticKeyword)),

                        //CreateConstructor(),
                        ConstructorDeclaration(
                                Identifier(ElementName))
                            .AddModifiers(CppVisibility.Private.ToCSharp())
                            .AddParameterListParameters(Parameter(Identifier(VariableName)).WithType(IdentifierName(nameof(IntPtr))))
                            .WithExpressionBody(
                                ArrowExpressionClause(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName(FieldName),
                                        IdentifierName(VariableName))))
                            .WithSemicolonToken(
                                Token(SyntaxKind.SemicolonToken)),
                        MethodDeclaration(
                                IdentifierName(ElementName),
                                Identifier("SetToNull"))
                            .WithExplicitInterfaceSpecifier(
                                ExplicitInterfaceSpecifier(
                                    GenericName(Identifier("IHandle"))
                                        .AddTypeArgumentListArguments(IdentifierName(ElementName))))
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
                                                        $"{ElementName}: 0x",
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
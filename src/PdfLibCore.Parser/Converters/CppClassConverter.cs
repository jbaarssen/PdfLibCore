using System;
using CppAst;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public partial class CppClassConverter : BaseCppConverter<CppClass>
{
    public CppClassConverter(CppClass cppElement)
        : base(cppElement)
    {
    }

    public override CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit)
    {
        return CppElement.IsDefinition
            ? CreateClassOrStruct(compilationUnit)
            : CreatePointerStruct(compilationUnit);
    }

    private static FieldDeclarationSyntax CreateField(CppField cppField)
    {
        return CreateField(cppField.Type.ToCSharp().ToString(), cppField.Name, cppField.Visibility);
    }

    private static FieldDeclarationSyntax CreateField(string type, string variable, CppVisibility visibility)
    {
        return FieldDeclaration(VariableDeclaration(IdentifierName(type))
                .AddVariables(VariableDeclarator(Identifier(variable))))
            .AddModifiers(visibility.ToCSharp());
    }

    private static PropertyDeclarationSyntax CreateProperty(string type, string name, CppVisibility visibility, Func<ArrowExpressionClauseSyntax> func)
    {
        return PropertyDeclaration(
                IdentifierName(type),
                Identifier(name))
            .AddModifiers(visibility.ToCSharp())
            .WithExpressionBody(func())
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private object CreateConstructor()
    {
        throw new System.NotImplementedException();
    }
}
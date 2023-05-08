using System;
using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public partial class CppClassConverter : BaseCppConverter<CppClass>
{
    public CppClassConverter(CppClass cppElement)
        : this(cppElement.Name, cppElement)
    {
    }

    public CppClassConverter(string name, CppClass cppElement)
        : base(name, cppElement)
    {
    }

    public override CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit)
    {
        return CppElement.IsDefinition
            ? CreateClassOrStruct(compilationUnit)
            : CreatePointerStruct(compilationUnit);
    }

    private static UsingDirectiveSyntax[] AddUsingDirectives(params string[] namespaces)
    {
        return namespaces
            .Select(namespaceName => CSharpSyntaxTree.ParseText($"using {namespaceName};"))
            .Select(tree => tree.GetRoot().ChildNodes().OfType<UsingDirectiveSyntax>().First())
            .ToArray();
    }

    private static FieldDeclarationSyntax CreateField(CppField cppField)
    {
        return CreateField(cppField.Type.ToCSharp().ToString(), NameHelper.Field.ToCSharp(cppField.Name), cppField.Visibility);
    }

    private static FieldDeclarationSyntax CreateField(string type, string variable, CppVisibility visibility)
    {
        return FieldDeclaration(VariableDeclaration(IdentifierName(type))
                .AddVariables(VariableDeclarator(Identifier(variable))))
            .AddModifiers(visibility.ToCSharp());
    }

    private static PropertyDeclarationSyntax CreateProperty(CppField cppField)
    {
        var decl = CreateProperty(cppField.Type.ToCSharp().ToString(), NameHelper.Field.ToCSharp(cppField.Name), cppField.Visibility);
        if (cppField.HasComments())
        {
            decl = decl.WithLeadingTrivia(cppField.Comment.Children.SelectMany(child => child.Children).Select(x => Comment($"// {x}")).ToArray());
        }
        return decl;
    }

    private static PropertyDeclarationSyntax CreateProperty(string type, string name, CppVisibility visibility, Func<ArrowExpressionClauseSyntax>? func = null)
    {
        var decl = PropertyDeclaration(IdentifierName(type), Identifier(name))
            .AddModifiers(visibility.ToCSharp());
        if (func != null)
        {
            decl = decl.WithExpressionBody(func()).WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
        }
        else
        {
            decl = decl.AddAccessorListAccessors(
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
        }
        return decl;
    }

    private object CreateConstructor()
    {
        throw new System.NotImplementedException();
    }
}
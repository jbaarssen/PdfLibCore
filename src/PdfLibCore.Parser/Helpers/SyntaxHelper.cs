using System;
using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Helpers;

public static class SyntaxHelper
{
    public static UsingDirectiveSyntax[] AddUsingDirectives(params string[] namespaces)
    {
        return namespaces
            .Select(namespaceName => CSharpSyntaxTree.ParseText($"using {namespaceName};"))
            .Select(tree => tree.GetRoot().ChildNodes().OfType<UsingDirectiveSyntax>().First())
            .ToArray();
    }

    public static FieldDeclarationSyntax CreateField(CppField cppField)
    {
        return CreateField(cppField.Type.ToCSharp().ToString(), NameHelper.Field.ToCSharp(cppField.Name), cppField.Visibility);
    }

    public static FieldDeclarationSyntax CreateField(string type, string variable, CppVisibility visibility)
    {
        return FieldDeclaration(VariableDeclaration(IdentifierName(type))
                .AddVariables(VariableDeclarator(Identifier(variable))))
            .AddModifiers(visibility.ToCSharp());
    }

    public static bool TryCreateProperty(CppField cppField, out PropertyDeclarationSyntax? property)
    {
        property = null;
        var type = cppField.Type.ToCSharp();
        if (type == null)
        {
            return false;
        }

        var decl = CreateProperty(type.ToString(), NameHelper.Field.ToCSharp(cppField.Name), cppField.Visibility);
        if (cppField.HasComments())
        {
            decl = decl.WithLeadingTrivia(cppField.Comment.Children.SelectMany(child => child.Children).Select(x => Comment($"// {x}")).ToArray());
        }
        property = decl;
        return true;
    }

    public static PropertyDeclarationSyntax CreateProperty(string type, string name, CppVisibility visibility, Func<ArrowExpressionClauseSyntax>? func = null)
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

    public static object CreateConstructor()
    {
        throw new System.NotImplementedException();
    }
}
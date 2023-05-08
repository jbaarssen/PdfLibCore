using System;
using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public partial class CppClassConverter
{
    private CompilationUnitSyntax CreateClassOrStruct(CompilationUnitSyntax compilationUnitSyntax)
    {
        compilationUnitSyntax = compilationUnitSyntax.AddUsings(AddUsingDirectives(
            "System",
            "System.Runtime.InteropServices"));

        var decl = CppElement.ClassKind switch
        {
            CppClassKind.Class => ClassDeclaration(ElementName),
            CppClassKind.Struct => (TypeDeclarationSyntax) StructDeclaration(ElementName),
            CppClassKind.Union => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };

        decl = decl.AddModifiers(CppElement.Visibility.ToCSharp());

        //[StructLayout(LayoutKind.Sequential)]
        decl = decl
            .WithAttributeLists(
                SingletonList(
                    AttributeList(
                        SingletonSeparatedList(
                            Attribute(IdentifierName("StructLayout"))
                                .WithArgumentList(
                                    AttributeArgumentList(
                                        SingletonSeparatedList(
                                            AttributeArgument(
                                                MemberAccessExpression(
                                                    SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("LayoutKind"),
                                                    IdentifierName("Sequential"))))))))));

        // Add class/struct comment(s)
        if (CppElement.HasComments())
        {
            decl = decl.WithLeadingTrivia(CppElement.Comment.Children.SelectMany(child => child.Children).Select(x => Comment($"// {x}")).ToArray());
        }

        // Add fields / properties
        var fields = CppElement.Fields.Select(CreateProperty).ToArray();
        if (fields.Any())
        {
            decl = decl.AddMembers(fields);
        }

        // Add constructors
        foreach (var ctor in CppElement.Constructors)
        {
        }

        // Add methods
        foreach (var method in CppElement.Functions)
        {
        }

        return compilationUnitSyntax
            .AddMembers(decl);
    }
}
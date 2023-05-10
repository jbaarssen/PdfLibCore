using System;
using System.Linq;
using System.Runtime.InteropServices;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using Serilog;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public class CppClassConverter : BaseCppConverter<CppClass>
{
    public CppClassConverter(CppClass cppElement, ILogger logger)
        : base(cppElement, logger)
    {
    }

    protected override CompilationUnitSyntax OnConvert(CompilationUnitSyntax compilationUnit)
    {
        compilationUnit = compilationUnit
            .AddUsings(SyntaxHelper.AddUsingDirectives("System", "System.Runtime.InteropServices"));

        var decl = CppElement.ClassKind switch
        {
            CppClassKind.Class => ClassDeclaration(ElementName),
            CppClassKind.Struct => (TypeDeclarationSyntax) StructDeclaration(ElementName),
            CppClassKind.Union => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };

        decl = decl.AddModifiers(CppElement.Visibility.ToCSharp());
        decl = decl.AddModifiers(Token(SyntaxKind.PartialKeyword));

        //[StructLayout(LayoutKind.Sequential)]
        decl = decl
            .AddAttributeLists(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(NameHelper.GetName<StructLayoutAttribute>())
                            .AddArgumentListArguments(
                                AttributeArgument(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName(nameof(LayoutKind)),
                                        IdentifierName(nameof(LayoutKind.Sequential))))))));

        // Add class/struct comment(s)
        if (CppElement.HasComments())
        {
            decl = decl.WithLeadingTrivia(CppElement.Comment.Children.SelectMany(child => child.Children).Select(x => Comment($"// {x}")).ToArray());
        }

        // Add fields / properties
        decl = CppElement.Fields
            .Aggregate(decl, (current, field) =>
            {
                if (SyntaxHelper.TryCreateProperty(field, out var property))
                {
                    current = current.AddMembers(property!);
                }
                return current;
            });

        return compilationUnit.AddMembers(decl);
    }
}
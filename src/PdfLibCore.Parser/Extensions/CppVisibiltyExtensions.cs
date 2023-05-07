using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

// ReSharper disable once CheckNamespace
namespace CppAst;

public static class CppVisibiltyExtensions
{
    public static SyntaxTokenList ToCSharp(this CppVisibility cppVisibility)
    {
        return TokenList(Token(cppVisibility switch
        {
            CppVisibility.Default => SyntaxKind.PublicKeyword,
            CppVisibility.Public => SyntaxKind.PublicKeyword,
            CppVisibility.Protected => SyntaxKind.ProtectedKeyword,
            CppVisibility.Private => SyntaxKind.PrivateKeyword,
            _ => throw new ArgumentException(cppVisibility.ToString())
        }));
    }
}
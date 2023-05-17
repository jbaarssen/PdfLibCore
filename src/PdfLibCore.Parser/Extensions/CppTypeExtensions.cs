using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

// ReSharper disable once CheckNamespace
namespace CppAst;

public static class CppTypeExtensions
{
    public static TypeSyntax? ToCSharp(this CppType cppType) => cppType switch
    {
        CppPrimitiveType t => t.Kind.Primitive(),
        CppClass t => IdentifierName(NameHelper.ToCSharp(t.Name)),
        CppEnum t => IdentifierName(NameHelper.ToCSharp(t.Name)),
        CppPointerType t => t.WithElementType(),
        CppArrayType => null,
        CppFunctionType => null,
        CppQualifiedType t => t.WithElementType(),
        CppReferenceType => null,
        CppTemplateParameterType => null,
        CppTemplateParameterNonType => null,
        CppTypedef t => t.Typedef(),
        CppTypeDeclaration => null,
        CppUnexposedType => null,
        _ => throw new NotSupportedException($"{cppType.TypeKind} not supported")
    };

    private static TypeSyntax? WithElementType(this CppTypeWithElementType type)
    {
        var elementType = type.ElementType switch
        {
            CppPrimitiveType { Kind: CppPrimitiveKind.Char } => CppPrimitiveKind.WChar.Primitive(),
            CppPrimitiveType { Kind: CppPrimitiveKind.Void } => IdentifierName(nameof(IntPtr)),
            CppQualifiedType q => q.WithElementType(),
            _ => type.ElementType.ToCSharp()
        };
        return elementType;
    }

    private static TypeSyntax? Typedef(this CppTypedef type)
    {
        return type.ElementType switch
        {
            CppPointerType { ElementType: CppQualifiedType or CppClass } => IdentifierName(type.GetDisplayName()),
            _ => type.ElementType.ToCSharp()
        };
    }

    private static TypeSyntax Primitive(this CppPrimitiveKind kind) => kind switch
    {
        CppPrimitiveKind.Void => IdentifierName(nameof(IntPtr)),
        CppPrimitiveKind.Bool => PredefinedType(Token(SyntaxKind.BoolKeyword)),
        CppPrimitiveKind.WChar => PredefinedType(Token(SyntaxKind.StringKeyword)),
        CppPrimitiveKind.Char => PredefinedType(Token(SyntaxKind.CharKeyword)),
        CppPrimitiveKind.Short => PredefinedType(Token(SyntaxKind.ShortKeyword)),
        CppPrimitiveKind.Int => PredefinedType(Token(SyntaxKind.IntKeyword)),
        CppPrimitiveKind.LongLong => PredefinedType(Token(SyntaxKind.LongKeyword)),
        CppPrimitiveKind.UnsignedChar => PredefinedType(Token(SyntaxKind.ByteKeyword)),
        CppPrimitiveKind.UnsignedShort => PredefinedType(Token(SyntaxKind.UShortKeyword)),
        CppPrimitiveKind.UnsignedInt => PredefinedType(Token(SyntaxKind.UIntKeyword)),
        CppPrimitiveKind.UnsignedLongLong => PredefinedType(Token(SyntaxKind.ULongKeyword)),
        CppPrimitiveKind.Float => PredefinedType(Token(SyntaxKind.FloatKeyword)),
        CppPrimitiveKind.Double => PredefinedType(Token(SyntaxKind.DoubleKeyword)),
        CppPrimitiveKind.LongDouble => PredefinedType(Token(SyntaxKind.DecimalKeyword)),
        _ => throw new NotSupportedException($"{kind} not supported")
    };
}
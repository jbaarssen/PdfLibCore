using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

// ReSharper disable once CheckNamespace
namespace CppAst;

public static class CppTypeExtensions
{
    public static TypeSyntax ToCSharp(this CppType cppType) => cppType switch
    {
        CppPrimitiveType t => t.Primitive(),
        CppPointerType t => t.Pointer(),
        CppArrayType t => throw new NotImplementedException(),
        CppFunctionType t => t.Function(),
        CppQualifiedType t => t.Qualified(),
        CppReferenceType t => throw new NotImplementedException(),
        CppTemplateParameterType t => throw new NotImplementedException(),
        CppTemplateParameterNonType t => throw new NotImplementedException(),
        CppTypedef t => t.Typedef(),
        CppTypeDeclaration t => throw new NotImplementedException(),
        CppUnexposedType t => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{cppType.TypeKind} not supported")
    };

    private static TypeSyntax Primitive(this CppPrimitiveType type) =>
        type.Kind.Primitive();

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

    private static TypeSyntax Pointer(this CppPointerType type)
    {
        var elementType = type.ElementType.ToCSharp();
        elementType = type.ElementType switch
        {
            CppPrimitiveType { Kind: CppPrimitiveKind.Char } => CppPrimitiveKind.WChar.Primitive(),
            CppPrimitiveType { Kind: CppPrimitiveKind.Void } => IdentifierName(nameof(IntPtr)),
            _ => elementType
        };
        return elementType;
    }

    private static TypeSyntax Qualified(this CppQualifiedType type)
    {
        return type.ElementType.ToCSharp();
    }

    private static TypeSyntax Function(this CppFunctionType type)
    {
        return type.ReturnType.ToCSharp();
    }

    private static TypeSyntax Typedef(this CppTypedef type)
    {
        return type.ElementType.ToCSharp();
    }

}
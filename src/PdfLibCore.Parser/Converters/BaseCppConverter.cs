using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public abstract class BaseCppConverter<T> : ICppConverter
    where T : ICppElement, ICppMemberWithVisibility
{
    protected T CppElement { get; }

    protected BaseCppConverter(T cppElement)
    {
        CppElement = cppElement;
    }

    public abstract CompilationUnitSyntax Convert(CompilationUnitSyntax compilationUnit);

    protected static ExpressionSyntax? ToExpressionSyntax(CppExpression? expression) => expression == null
        ? null
        : expression.Kind switch
        {
            CppExpressionKind.IntegerLiteral => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(ChangeType<int>(expression))),
            CppExpressionKind.FloatingLiteral => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(ChangeType<float>(expression))),
            CppExpressionKind.FixedPointLiteral => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(ChangeType<double>(expression))),
            CppExpressionKind.Unexposed => throw new NotImplementedException(nameof(CppExpressionKind.Unexposed)),
            CppExpressionKind.DeclRef => IdentifierName(expression.ToString()!),
            CppExpressionKind.MemberRef => throw new NotImplementedException(nameof(CppExpressionKind.MemberRef)),
            CppExpressionKind.Call => throw new NotImplementedException(nameof(CppExpressionKind.Call)),
            CppExpressionKind.ObjCMessage => throw new NotImplementedException(nameof(CppExpressionKind.ObjCMessage)),
            CppExpressionKind.Block => throw new NotImplementedException(nameof(CppExpressionKind.Block)),
            CppExpressionKind.ImaginaryLiteral => throw new NotImplementedException(nameof(CppExpressionKind.ImaginaryLiteral)),
            CppExpressionKind.StringLiteral => throw new NotImplementedException(nameof(CppExpressionKind.StringLiteral)),
            CppExpressionKind.CharacterLiteral => throw new NotImplementedException(nameof(CppExpressionKind.CharacterLiteral)),
            CppExpressionKind.Paren => throw new NotImplementedException(nameof(CppExpressionKind.Paren)),
            CppExpressionKind.UnaryOperator => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(ChangeType<int>(expression))),
            CppExpressionKind.ArraySubscript => throw new NotImplementedException(nameof(CppExpressionKind.ArraySubscript)),
            CppExpressionKind.BinaryOperator => throw new NotImplementedException(nameof(CppExpressionKind.BinaryOperator)),
            CppExpressionKind.CompoundAssignOperator => throw new NotImplementedException(nameof(CppExpressionKind.CompoundAssignOperator)),
            CppExpressionKind.ConditionalOperator => throw new NotImplementedException(nameof(CppExpressionKind.ConditionalOperator)),
            CppExpressionKind.CStyleCast => throw new NotImplementedException(nameof(CppExpressionKind.CStyleCast)),
            CppExpressionKind.CompoundLiteral => throw new NotImplementedException(nameof(CppExpressionKind.CompoundLiteral)),
            CppExpressionKind.InitList => throw new NotImplementedException(nameof(CppExpressionKind.InitList)),
            CppExpressionKind.AddrLabel => throw new NotImplementedException(nameof(CppExpressionKind.AddrLabel)),
            CppExpressionKind.Stmt => throw new NotImplementedException(nameof(CppExpressionKind.Stmt)),
            CppExpressionKind.GenericSelection => throw new NotImplementedException(nameof(CppExpressionKind.GenericSelection)),
            CppExpressionKind.GNUNull => throw new NotImplementedException(nameof(CppExpressionKind.GNUNull)),
            CppExpressionKind.CXXStaticCast => throw new NotImplementedException(nameof(CppExpressionKind.CXXStaticCast)),
            CppExpressionKind.CXXDynamicCast => throw new NotImplementedException(nameof(CppExpressionKind.CXXDynamicCast)),
            CppExpressionKind.CXXReinterpretCast => throw new NotImplementedException(nameof(CppExpressionKind.CXXReinterpretCast)),
            CppExpressionKind.CXXConstCast => throw new NotImplementedException(nameof(CppExpressionKind.CXXConstCast)),
            CppExpressionKind.CXXFunctionalCast => throw new NotImplementedException(nameof(CppExpressionKind.CXXFunctionalCast)),
            CppExpressionKind.CXXTypeid => throw new NotImplementedException(nameof(CppExpressionKind.CXXTypeid)),
            CppExpressionKind.CXXBoolLiteral => throw new NotImplementedException(nameof(CppExpressionKind.CXXBoolLiteral)),
            CppExpressionKind.CXXNullPtrLiteral => throw new NotImplementedException(nameof(CppExpressionKind.CXXNullPtrLiteral)),
            CppExpressionKind.CXXThis => throw new NotImplementedException(nameof(CppExpressionKind.CXXThis)),
            CppExpressionKind.CXXThrow => throw new NotImplementedException(nameof(CppExpressionKind.CXXThrow)),
            CppExpressionKind.CXXNew => throw new NotImplementedException(nameof(CppExpressionKind.CXXNew)),
            CppExpressionKind.CXXDelete => throw new NotImplementedException(nameof(CppExpressionKind.CXXDelete)),
            CppExpressionKind.Unary => LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(ChangeType<int>(expression))),
            CppExpressionKind.ObjCStringLiteral => throw new NotImplementedException(nameof(CppExpressionKind.ObjCBoolLiteral)),
            CppExpressionKind.ObjCEncode => throw new NotImplementedException(nameof(CppExpressionKind.ObjCEncode)),
            CppExpressionKind.ObjCSelector => throw new NotImplementedException(nameof(CppExpressionKind.ObjCSelector)),
            CppExpressionKind.ObjCProtocol => throw new NotImplementedException(nameof(CppExpressionKind.ObjCProtocol)),
            CppExpressionKind.ObjCBridgedCast => throw new NotImplementedException(nameof(CppExpressionKind.ObjCBridgedCast)),
            CppExpressionKind.PackExpansion => throw new NotImplementedException(nameof(CppExpressionKind.PackExpansion)),
            CppExpressionKind.SizeOfPack => throw new NotImplementedException(nameof(CppExpressionKind.SizeOfPack)),
            CppExpressionKind.Lambda => throw new NotImplementedException(nameof(CppExpressionKind.Lambda)),
            CppExpressionKind.ObjCBoolLiteral => throw new NotImplementedException(nameof(CppExpressionKind.ObjCBoolLiteral)),
            CppExpressionKind.ObjCSelf => throw new NotImplementedException(nameof(CppExpressionKind.ObjCSelf)),
            CppExpressionKind.OMPArraySection => throw new NotImplementedException(nameof(CppExpressionKind.OMPArraySection)),
            CppExpressionKind.ObjCAvailabilityCheck => throw new NotImplementedException(nameof(CppExpressionKind.ObjCAvailabilityCheck)),
            _ => throw new ArgumentException(expression.Kind.ToString())
        };

    private static TType ChangeType<TType>(CppExpression value) =>
        (TType) System.Convert.ChangeType(value.ToString(), typeof(TType))!;
}
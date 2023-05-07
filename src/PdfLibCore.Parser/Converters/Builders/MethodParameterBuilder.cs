using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters.Builders;

public sealed class MethodParameterBuilder
{
    private readonly CppFunction _cppFunction;

    public MethodParameterBuilder(CppFunction cppFunction)
    {
        _cppFunction = cppFunction;
    }

    public ParameterListSyntax GetParameters()
    {
        var parameters = new List<SyntaxNodeOrToken>();
        foreach (var parameter in _cppFunction.Parameters)
        {
            parameters.Add(Parameter(Identifier(parameter.Name))
                .WithType(IdentifierName(parameter.Type.ToString()!)));
            parameters.Add(Token(SyntaxKind.CommaToken));
        }

        if (parameters.Any())
        {
            parameters.RemoveAt(parameters.Count - 1);
        }
        return ParameterList(SeparatedList<ParameterSyntax>(parameters.ToArray()));
    }
}
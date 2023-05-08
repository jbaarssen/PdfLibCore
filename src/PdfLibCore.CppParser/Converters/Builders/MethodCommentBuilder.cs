using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.CppParser.Converters.Builders;

public sealed class MethodCommentBuilder
{
    private readonly CppFunction _cppFunction;

    private enum CommentType
    {
        None,
        Summary,
        Remarks,
        Parameter,
        Returns
    }

    private readonly Dictionary<CommentType, List<string>> _comments = new()
    {
        { CommentType.Summary, new List<string>() },
        { CommentType.Remarks, new List<string>() },
        { CommentType.Parameter, new List<string>() },
        { CommentType.Returns, new List<string>() }
    };

    public MethodCommentBuilder(CppFunction cppFunction)
    {
        _cppFunction = cppFunction;
        Filter();
    }

    public SyntaxTriviaList GetComments()
    {
        var commentNodes = new List<XmlNodeSyntax>();

        ParseComments(commentNodes);
        ParseParameters(commentNodes);
        ParseRemarks(commentNodes);
        ParseReturns(commentNodes);

        if (!commentNodes.Any() || commentNodes.Last().ToString() != Environment.NewLine)
        {
            return TriviaList();
        }

        commentNodes.RemoveAt(commentNodes.Count - 1);
        commentNodes.Add(XmlText().WithTextTokens(
            TokenList(
                Token(
                    TriviaList(),
                    SyntaxKind.XmlTextLiteralNewLineToken,
                    Environment.NewLine,
                    Environment.NewLine,
                    TriviaList()
                )
            )
        ));

        return TriviaList(Trivia(DocumentationComment(commentNodes.ToArray())));
    }

    private void ParseComments(ICollection<XmlNodeSyntax> commentNodes)
    {
        if (!_comments[CommentType.Summary].Any())
        {
            return;
        }
        commentNodes.Add(XmlSummaryElement(GetValue(_comments[CommentType.Summary])));
        commentNodes.Add(XmlNewLine(Environment.NewLine));
    }

    private void ParseParameters(ICollection<XmlNodeSyntax> commentNodes)
    {
        if (!_comments[CommentType.Parameter].Any())
        {
            return;
        }

        var parameters = new Dictionary<string, List<string>>();
        var key = string.Empty;
        foreach (var p in _comments[CommentType.Parameter])
        {
            if (p.Contains(" - "))
            {
                var parts = p.Split(" - ");
                key = parts[0].Trim();
                parameters[key] = new List<string> { parts[1].Trim() };
            }
            else if (!string.IsNullOrWhiteSpace(key))
            {
                parameters[key].Add(p);
            }
        }

        foreach (var kvp in parameters)
        {
            commentNodes.Add(XmlParamElement(kvp.Key, GetValue(kvp.Value)));
            commentNodes.Add(XmlNewLine(Environment.NewLine));
        }
    }

    private void ParseRemarks(ICollection<XmlNodeSyntax> commentNodes)
    {
        if (!_comments[CommentType.Remarks].Any())
        {
            return;
        }
        commentNodes.Add(XmlRemarksElement(GetValue(_comments[CommentType.Remarks])));
        commentNodes.Add(XmlNewLine(Environment.NewLine));
    }

    private void ParseReturns(ICollection<XmlNodeSyntax> commentNodes)
    {
        if (!_comments[CommentType.Returns].Any() ||
            _comments[CommentType.Returns].Count == 1 &&
            _comments[CommentType.Returns].Any(x => x.StartsWith("none", StringComparison.InvariantCultureIgnoreCase)))
        {
            return;
        }

        commentNodes.Add(XmlReturnsElement(GetValue(_comments[CommentType.Returns])));
        commentNodes.Add(XmlNewLine(Environment.NewLine));
    }

    private static XmlNodeSyntax[] GetValue(IReadOnlyList<string> values)
    {
        var list = new List<XmlNodeSyntax>();
        if (values.Count == 1)
        {
            list.Add(XmlText(values[0].Trim()));
        }
        else
        {
            list.Add(XmlNewLine(Environment.NewLine));
            list.AddRange(values.Select(x => XmlText($" {x.Trim()}").AddTextTokens(XmlTextNewLine(Environment.NewLine))).ToArray());
        }
        return list.ToArray();
    }

    private void Filter()
    {
        var commentType = CommentType.None;
        foreach (var text in _cppFunction.Comment.Children.SelectMany(child => child.Children).Select(x => x.ToString()))
        {
            if (text.StartsWith("Function:", StringComparison.InvariantCultureIgnoreCase))
            {
                commentType = CommentType.Summary;
                continue;
            }
            if (text.StartsWith("Parameters:", StringComparison.InvariantCultureIgnoreCase))
            {
                commentType = CommentType.Parameter;
                continue;
            }
            if (text.StartsWith("Return value:", StringComparison.InvariantCultureIgnoreCase))
            {
                commentType = CommentType.Returns;
                continue;
            }
            if (text.StartsWith("Comments:", StringComparison.InvariantCultureIgnoreCase))
            {
                commentType = CommentType.Remarks;
                continue;
            }

            if (commentType != CommentType.None)
            {
                _comments[commentType].Add(text);
            }
        }
    }
}
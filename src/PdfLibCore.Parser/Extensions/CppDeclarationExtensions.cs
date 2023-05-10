using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

// ReSharper disable once CheckNamespace
namespace CppAst;

public static class CppDeclarationExtensions
{
    private enum CommentType
    {
        None,
        Summary,
        Remarks,
        Parameter,
        Returns
    }

    public static bool HasComments<T>(this T cppClass) where T : ICppDeclaration =>
        cppClass.Comment != null;

    public static SyntaxTriviaList GetComments<T>(this T cppElement)
        where T : CppElement, ICppDeclaration
    {
        var filtered = Filter(cppElement);
        var commentNodes = new List<XmlNodeSyntax>();

        ParseComments(filtered, commentNodes);
        ParseParameters(filtered, commentNodes);
        ParseRemarks(filtered, commentNodes, cppElement);
        ParseReturns(filtered, commentNodes);

        if (!commentNodes.Any() || commentNodes.Last().ToString() != Environment.NewLine)
        {
            return TriviaList();
        }

        commentNodes.RemoveAt(commentNodes.Count - 1);
        commentNodes.Add(XmlText().AddTextTokens(Token(
            TriviaList(),
            SyntaxKind.XmlTextLiteralNewLineToken,
            Environment.NewLine,
            Environment.NewLine,
            TriviaList()
        )));

        return TriviaList(Trivia(DocumentationComment(commentNodes.ToArray())));
    }

    private static void ParseComments(IReadOnlyDictionary<CommentType, List<string>> comments, ICollection<XmlNodeSyntax> commentNodes)
    {
        if (!comments[CommentType.Summary].Any())
        {
            return;
        }
        commentNodes.Add(XmlSummaryElement(GetValue(comments[CommentType.Summary])));
        commentNodes.Add(XmlNewLine(Environment.NewLine));
    }

    private static void ParseParameters(IReadOnlyDictionary<CommentType, List<string>> comments, ICollection<XmlNodeSyntax> commentNodes)
    {
        if (!comments[CommentType.Parameter].Any())
        {
            return;
        }

        var parameters = new Dictionary<string, List<string>>();
        var key = string.Empty;
        foreach (var p in comments[CommentType.Parameter])
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

    private static void ParseRemarks<T>(IReadOnlyDictionary<CommentType, List<string>> comments, ICollection<XmlNodeSyntax> commentNodes, T cppElement)
        where T : CppElement, ICppDeclaration
    {
        var lines = GetValue(comments[CommentType.Remarks]).ToList();

        if (!string.IsNullOrWhiteSpace(cppElement.SourceFile))
        {
            var link = $"For more information see: https://pdfium.googlesource.com/pdfium/+/master/public/{Path.GetFileName(cppElement.SourceFile)}.";
            if (lines.Count > 1)
            {
                lines.Add(XmlText($" {link}").AddTextTokens(XmlTextNewLine(Environment.NewLine)));
            }
            else
            {
                lines[0] = XmlText(link);
            }
        }
        commentNodes.Add(XmlRemarksElement(lines.ToArray()));
        commentNodes.Add(XmlNewLine(Environment.NewLine));
    }

    private static void ParseReturns(IReadOnlyDictionary<CommentType, List<string>> comments, ICollection<XmlNodeSyntax> commentNodes)
    {
        if (!comments[CommentType.Returns].Any() ||
            comments[CommentType.Returns].Count == 1 &&
            comments[CommentType.Returns].Any(x => x.StartsWith("none", StringComparison.InvariantCultureIgnoreCase)))
        {
            return;
        }

        commentNodes.Add(XmlReturnsElement(GetValue(comments[CommentType.Returns])));
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

    private static Dictionary<CommentType, List<string>> Filter(ICppDeclaration function)
    {
        var comments = new Dictionary<CommentType, List<string>>()
        {
            { CommentType.Summary, new List<string>() },
            { CommentType.Remarks, new List<string>() },
            { CommentType.Parameter, new List<string>() },
            { CommentType.Returns, new List<string>() }
        };

        var commentType = CommentType.None;
        foreach (var text in function.Comment.Children.SelectMany(child => child.Children).Select(x => x.ToString()))
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
                comments[commentType].Add(text);
            }
        }
        return comments;
    }
}
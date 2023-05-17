using System;
using System.Collections.Generic;
using System.Text;

namespace PdfLibCore.CppSharp.Helpers;

public static class CommentHelper
{
    private enum CommentType
    {
        None,
        Summary,
        Remarks,
        Parameter,
        Returns
    }

    public static string Replace(string fileContent)
    {
        var lines = fileContent.Split(Environment.NewLine);
        var sb = new StringBuilder(fileContent);
        for (var index = lines.Length - 1; index >= 0; index--)
        {
            if (!lines[index].Contains(CommentBuilder.Separator))
            {
                continue;
            }
            sb.Replace(lines[index], Build(lines[index]));
        }
        return sb.ToString();
    }

    private static string Build(string comment)
    {
        var padding = comment.IndexOf("///", StringComparison.InvariantCultureIgnoreCase);

        var groups = Group(comment);
        return new CommentBuilder(padding)
            .AddSummary(groups[CommentType.Summary])
            .AddParameters(groups[CommentType.Parameter])
            .AddRemarks(groups[CommentType.Remarks])
            .AddReturns(groups[CommentType.Returns]);
    }

    private static Dictionary<CommentType, List<string>> Group(string comment)
    {
        var comments = new Dictionary<CommentType, List<string>>()
        {
            { CommentType.Summary, new List<string>() },
            { CommentType.Remarks, new List<string>() },
            { CommentType.Parameter, new List<string>() },
            { CommentType.Returns, new List<string>() }
        };

        var commentType = CommentType.None;
        comment = comment
            .Replace("///", string.Empty)
            .Replace("<summary>", string.Empty)
            .Replace("</summary>", string.Empty)
            .Trim();
        foreach (var text in comment.Split(CommentBuilder.Separator))
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
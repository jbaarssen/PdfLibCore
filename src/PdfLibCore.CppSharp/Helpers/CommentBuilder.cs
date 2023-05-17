using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfLibCore.CppSharp.Helpers;

public class CommentBuilder
{
    public static string Separator => "{{PDFLIBCORE}}";

    private readonly int _padding;
    private readonly char _paddingChar;

    private readonly StringBuilder _builder = new();

    public CommentBuilder(int padding, char paddingChar = ' ')
    {
        _padding = padding;
        _paddingChar = paddingChar;
    }

    public CommentBuilder AddSummary(List<string> lines) =>
        Add("summary", lines);

    public CommentBuilder AddRemarks(List<string> lines) =>
        Add("remarks", lines);

    public CommentBuilder AddReturns(List<string> lines) =>
        lines.Count == 1 && lines[0].Contains("none", StringComparison.InvariantCultureIgnoreCase)
            ? this
            : Add("returns", lines);

    public CommentBuilder AddParameters(List<string> lines)
    {
        if (!lines.Any())
        {
            return this;
        }

        var descriptions = new Dictionary<string, List<string>>();
        foreach (var line in lines)
        {
            if (!line.Contains(" - "))
            {
                var last = descriptions.LastOrDefault();
                if (string.IsNullOrEmpty(last.Key))
                {
                    continue;
                }
                last.Value.Add(line);
                continue;
            }

            var parts = line.Split(" - ");
            descriptions[parts[0].Trim()] = new List<string> { parts[1].Trim() };
        }

        foreach (var (key, data) in descriptions)
        {
            Add("param", data, $"name=\"{key}\"");
        }
        return this;
    }

    private void AppendLine(string line) =>
        _builder.AppendLine($"{"".PadLeft(_padding, _paddingChar)}/// {line}");

    public override string ToString() =>
        _builder.ToString().TrimEnd('\r', '\n');

    private CommentBuilder Add(string tagName, List<string> lines, string attribute = null)
    {
        if (!lines.Any())
        {
            return this;
        }

        var startTag = $"<{tagName}{(string.IsNullOrEmpty(attribute) ? string.Empty : $" {attribute}")}>";
        var endTag = $"</{tagName}>";

        if (lines.Count == 2)
        {
            var joined = string.Join(" ", lines);
            if (joined.Length <= 100)
            {
                lines.Clear();
                lines.Add(joined);
            }
        }

        if (lines.Count == 1)
        {
            AppendLine($"{startTag}{lines[0]}{endTag}");
            return this;
        }

        AppendLine(startTag);
        foreach (var line in lines)
        {
            AppendLine(line);
        }
        AppendLine(endTag);
        return this;
    }

    public static implicit operator string(CommentBuilder builder) =>
        builder.ToString();
}
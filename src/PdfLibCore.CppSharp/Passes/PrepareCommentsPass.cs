using System.Collections.Generic;
using System.IO;
using System.Linq;
using CppSharp.AST;
using CppSharp.Passes;
using PdfLibCore.CppSharp.Helpers;

namespace PdfLibCore.CppSharp.Passes;

public class PrepareCommentsPass : TranslationUnitPass
{
    private readonly Dictionary<string, string[]> _headerFiles = new();

    public PrepareCommentsPass()
    {
        VisitOptions.ResetFlags(VisitFlags.ClassBases | VisitFlags.FunctionReturnType | VisitFlags.TemplateArguments);
    }

    public override bool VisitDeclaration(Declaration decl)
    {
        if (AlreadyVisited(decl))
        {
            return false;
        }

        if (decl is not (Function and not Method))
        {
            return true;
        }

        decl.Comment = new RawComment
        {
            FullComment = new FullComment
            {
                Blocks = new List<BlockContentComment>
                {
                    new ParagraphComment
                    {
                        Content = new List<InlineContentComment>
                        {
                            new TextComment
                            {
                                Text = GetComments(decl)
                            }
                        }
                    }
                }
            }
        };

        return true;
    }

    private string GetComments(Declaration declaration)
    {
        var s = (TranslationUnit) declaration.OriginalNamespace;
        if (!_headerFiles.ContainsKey(s.FilePath))
        {
            _headerFiles.Add(s.FilePath, File.ReadAllLines(s.FilePath));
        }

        var commentLines = new Stack<string>();

        var lineIndex = declaration.LineNumberStart - 2;
        bool complete;
        do
        {
            var line = _headerFiles[s.FilePath][lineIndex];
            commentLines.Push(line);

            complete = line.StartsWith("// Function: ") || string.IsNullOrWhiteSpace(line);
            lineIndex--;
        }
        while (!complete);

        return string.Join(CommentBuilder.Separator, commentLines.Where(c => c.Length > 2).Select(c => c[2..].TrimStart()));
    }
}
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CppAst;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PdfLibCore.Parser.Helpers;
using Serilog;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace PdfLibCore.Parser.Converters;

public sealed partial class Converter
{
    private readonly ILogger _logger;
    private readonly CppCompilation _compilation;
    private const string PdfLibCoreNamespace = "PdfLibCore.Generated";

    public CppDiagnosticBag Diagnostics => _compilation.Diagnostics;

    public Converter(ILogger logger, IEnumerable<Source> files)
    {
        _logger = logger;
        _compilation = CppParser.ParseFiles(files.Select(f => f.FullPath).ToList(), new CppParserOptions
        {
            ParseMacros = true,
            ParseAttributes = false
        });
    }

    public IEnumerable<CompiledSource> Convert()
    {
        foreach (var element in GetElements())
        {
            var converter = CppConverterFactory.GetCppConverter(element, _logger);
            if (converter != null)
            {
                yield return new CompiledSource(element, converter.Convert(GetCompilationUnit(element)));
            }
        }

        foreach(var file in CreatePdfium())
        {
            yield return file;
        }
    }

    private IEnumerable<SourceElement> GetElements()
    {
        foreach (var cppEnum in _compilation.Enums)
        {
            yield return new SourceElement(cppEnum.Name, cppEnum, "Enums");
        }
        foreach (var cppClass in _compilation.Classes.Where(c => c.IsDefinition))
        {
            yield return new SourceElement(cppClass.Name, cppClass, "Structs");
        }
        foreach (var cppTypedef in _compilation.Typedefs)
        {
            yield return new SourceElement(cppTypedef.Name, cppTypedef, "Types");
        }

        var sorter = new MacroSorter(_compilation.Macros);
        foreach (var macroGroup in sorter.GetGroups())
        {
            if (macroGroup.All(i => int.TryParse(i.Value.Value, out _)))
            {
                var cppEnum = new CppEnum(macroGroup.Name);
                cppEnum.Items.AddRange(macroGroup.Select(i => new CppEnumItem(i.Value.Name, string.IsNullOrEmpty(i.Value.Value) ? 0 : int.Parse(i.Value.Value))));
                yield return new SourceElement(macroGroup.Name, cppEnum, "Enums");
            }
        }
    }

    private static CompilationUnitSyntax GetCompilationUnit(SourceElement? element = null)
    {
        var now = DateTime.UtcNow;
        var trivia = new List<SyntaxTrivia>
        {
            Comment($@"/*
This file is part of PdfLibCore, a wrapper around the PDFium library for the .NET.
Inspired by the awesome work of PDFiumSharp by Tobias Meyer.

Copyright (C) {now.Year} J.C.A. Kokenberg & Jan Baarssen
License: Microsoft Reciprocal License (MS-RL)
*/
"),
            Comment($"// AUTOGENERATED FILE - {now:dd-MM-yyyy} ({now:hh:mm:ss}) - {now.Kind}"),
            Comment($"// DO NOT MODIFY{(element?.Element == null ? Environment.NewLine : string.Empty)}")
        };
        if (element?.Element != null)
        {
            trivia.Add(Comment($"// Sourcefile: https://pdfium.googlesource.com/pdfium/+/master/public/{Path.GetFileName(element.Element.SourceFile)}{Environment.NewLine}"));
        }

        var ns = element?.Path != null ? $"{PdfLibCoreNamespace}.{element.Path}" : PdfLibCoreNamespace;
        return CompilationUnit()
            .AddMembers(FileScopedNamespaceDeclaration(IdentifierName(ns))
                .WithNamespaceKeyword(
                    Token(TriviaList(trivia.ToArray()), SyntaxKind.NamespaceKeyword, TriviaList())));
    }



}
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Parser;
using CppSharp.Passes;
using PdfLibCore.CppSharp.Models;
using PdfLibCore.CppSharp.Passes;
using Module = CppSharp.AST.Module;

namespace PdfLibCore.CppSharp;

internal class Library : ILibrary
{
    private readonly string _directoryName;
    private readonly RunnerOptions _options;

    public Library(string directoryName, RunnerOptions options)
    {
        _directoryName = directoryName;
        _options = options;
    }

    public void Preprocess(Driver driver, ASTContext ctx)
    {
        PdfTypeMap.Register(driver.Context.TypeMaps.TypeMaps);
    }

    public void Postprocess(Driver driver, ASTContext ctx)
    {
        foreach (var function in ctx.TranslationUnits.SelectMany(t => t.Functions))
        {
            function.Name = function.LogicalOriginalName;
        }

        // Fix for generating code which will not compile.
        ctx.FindCompleteClass("FPDF_LIBRARY_CONFIG_")
            .Properties
            .First(f => f.OriginalName == "m_pUserFontPaths")
            .Ignore = true;
    }

    public void Setup(Driver driver)
    {
        driver.ParserOptions.Verbose = false;
        driver.ParserOptions.LanguageVersion = LanguageVersion.CPP17;
        driver.ParserOptions.SetupMSVC(VisualStudioVersion.Latest);

        driver.Options.UseSpan = true;
        driver.Options.GeneratorKind = GeneratorKind.CSharp;
        driver.Options.CommentKind = CommentKind.BCPLSlash;
        driver.Options.GenerationOutputMode = GenerationOutputMode.FilePerUnit;
        driver.Options.OutputDir = _directoryName;
        driver.Options.GetClassGenerationOptions = _ => new ClassGenerationOptions
        {
            GenerateNativeToManaged = false
        };

        var module = driver.Options.AddModule(_options.GeneratedProjectName);
        module.SharedLibraryName = _options.SharedLibraryName;
        module.Undefines.Add("_WIN32");

        module.IncludeDirs.Add(Path.Combine(_options.SolutionDir, "Native"));
        module.IncludeDirs.Add(Path.Combine(_directoryName, "include"));
        foreach (var includeDir in module.IncludeDirs)
        {
            module.Headers.AddRange(Directory.GetFiles(includeDir, "*.h").Select(Path.GetFileName));
        }
    }

    public void SetupPasses(Driver driver)
    {
        driver.AddTranslationUnitPass(new PrepareCommentsPass());
        driver.AddTranslationUnitPass(new RenameClasses());
        driver.AddTranslationUnitPass(new FixMethods());

        driver.AddGeneratorOutputPass(new RenameGeneratedClasses(_options));
        driver.AddGeneratorOutputPass(new AddSafePointerInterface());
    }
}
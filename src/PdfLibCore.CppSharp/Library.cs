using System.IO;
using System.Linq;
using CppSharp;
using CppSharp.AST;
using CppSharp.Generators;
using CppSharp.Parser;
using CppSharp.Types;
using PdfLibCore.CppSharp.Config;
using PdfLibCore.CppSharp.Models;
using PdfLibCore.CppSharp.Passes;
using YamlDotNet.Serialization;

namespace PdfLibCore.CppSharp;

internal class Library : ILibrary
{
    private readonly string _directoryName;
    private readonly RunnerOptions _options;
    private readonly Configuration _configuration;

    public Library(string directoryName, RunnerOptions options)
    {
        _directoryName = directoryName;
        _options = options;

        var configStream = GetType().Assembly.GetManifestResourceStream($"{typeof(Runner).Namespace}.config.yml")!;
        using var sr = new StreamReader(configStream);
        var x = sr.ReadToEnd();
        var deserializer = new DeserializerBuilder().Build();
        _configuration = deserializer.Deserialize<Configuration>(x);
        }

    public void Preprocess(Driver driver, ASTContext ctx)
    {
        PdfTypeMap.Register(driver.Context.TypeMaps);
    }

    public void Postprocess(Driver driver, ASTContext ctx)
    {
        foreach (var function in ctx.TranslationUnits.SelectMany(t => t.Functions))
        {
            function.Name = function.LogicalOriginalName;
            CheckParameters(function, driver.Context.TypeMaps);
        }

        CheckClasses(ctx);
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

        driver.AddGeneratorOutputPass(new RenameGeneratedClasses(_options));
        driver.AddGeneratorOutputPass(new AddSafePointerInterface());
    }

     private void CheckParameters(Function function, TypeMapDatabase typeMaps)
    {
        if (!_configuration.Methods.TryGetValue(function.Name, out var method))
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(method.ReturnType) && typeMaps.FindTypeMap(method.ReturnType, out var returnType))
        {
            function.ReturnType = new QualifiedType(returnType.Type);
        }

        foreach (var funcParam in function.Parameters.Where(p => method.Parameters.ContainsKey(p.Name)))
        {
            var param = method.Parameters[funcParam.Name];

            funcParam.Usage = param.Usage;
            funcParam.QualifiedType = typeMaps.FindTypeMap(param.Type ?? string.Empty, out var map)
                ? new QualifiedType(map.Type)
                : funcParam.QualifiedType;
        }
    }

    private void CheckClasses(ASTContext ctx)
    {
        foreach (var @class in ctx.TranslationUnits.SelectMany(t => t.Classes))
        {
            if (!_configuration.Classes.TryGetValue(@class.Name, out var foundClass))
            {
                continue;
            }

            foreach (var property in @class.Properties)
            {
                if (foundClass.Properties.TryGetValue(property.OriginalName, out var foundProperty))
                {
                    property.Ignore = foundProperty.Ignore;
                }
            }
        }
    }
}
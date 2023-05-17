using System.Threading.Tasks;
using PdfLibCore.CppSharp.Models;
using Serilog;

namespace PdfLibCore.CppSharp;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        // Create logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(theme: ConsoleTheme.ColorScheme)
            .CreateLogger();

        var runner = new Runner(Log.Logger, new RunnerOptions());
        await runner.RunAsync();
    }


}
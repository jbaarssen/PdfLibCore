using System;
using System.Collections.Generic;
using Serilog.Sinks.SystemConsole.Themes;

namespace PdfLibCore.Parser;

public static class ConsoleTheme
{
    public static SystemConsoleTheme ColorScheme { get; } = new(
        new Dictionary<ConsoleThemeStyle, SystemConsoleThemeStyle>
        {
            [ConsoleThemeStyle.Text] = new() { Foreground = ConsoleColor.Gray },
            [ConsoleThemeStyle.SecondaryText] = new() { Foreground = ConsoleColor.DarkGray },
            [ConsoleThemeStyle.TertiaryText] = new() { Foreground = ConsoleColor.DarkGray },
            [ConsoleThemeStyle.Invalid] = new() { Foreground = ConsoleColor.Yellow },
            [ConsoleThemeStyle.Null] = new() { Foreground = ConsoleColor.Cyan },
            [ConsoleThemeStyle.Name] = new() { Foreground = ConsoleColor.Cyan },
            [ConsoleThemeStyle.String] = new() { Foreground = ConsoleColor.Cyan },
            [ConsoleThemeStyle.Number] = new() { Foreground = ConsoleColor.Yellow },
            [ConsoleThemeStyle.Boolean] = new() { Foreground = ConsoleColor.Cyan },
            [ConsoleThemeStyle.Scalar] = new() { Foreground = ConsoleColor.Yellow },
            [ConsoleThemeStyle.LevelVerbose] = new() { Foreground = ConsoleColor.Gray, Background = ConsoleColor.DarkGray },
            [ConsoleThemeStyle.LevelDebug] = new() { Foreground = ConsoleColor.White, Background = ConsoleColor.DarkGray },
            [ConsoleThemeStyle.LevelInformation] = new() { Foreground = ConsoleColor.White, Background = ConsoleColor.Blue },
            [ConsoleThemeStyle.LevelWarning] = new() { Foreground = ConsoleColor.DarkGray, Background = ConsoleColor.Yellow },
            [ConsoleThemeStyle.LevelError] = new() { Foreground = ConsoleColor.White, Background = ConsoleColor.Red },
            [ConsoleThemeStyle.LevelFatal] = new() { Foreground = ConsoleColor.White, Background = ConsoleColor.DarkRed },
        });
}
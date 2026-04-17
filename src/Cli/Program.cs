using System.Diagnostics.CodeAnalysis;
using CommandLine;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Util;

namespace YSharp.Cli;

[ExcludeFromCodeCoverage]
internal static class Program
{
    private static void Main(string[] args)
    {
        CommandLine
            .Parser.Default.ParseArguments<CliArgs>(args)
            .WithParsed(cliargs =>
            {
                if (cliargs.ScriptPath is null)
                    Runner.ConsoleRunner(cliargs, new RuntimeEnviroment());
                else
                    Runner.ScriptRunner(cliargs.ScriptPath, cliargs, new RuntimeEnviroment());
            });
    }
}

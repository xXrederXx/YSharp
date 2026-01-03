using CommandLine;
using YSharp.Common;

namespace YSharp.Cli;

internal static class Program
{
    private static void Main(string[] args)
    {
        CommandLine
            .Parser.Default.ParseArguments<CliArgs>(args)
            .WithParsed(cliargs =>
            {
                ArgsHolder.UserArgs = cliargs;

                if (cliargs.ScriptPath is null)
                    Runner.ConsoleRunner();
                else
                    Runner.ScriptRunner(cliargs.ScriptPath);
            });
    }
}

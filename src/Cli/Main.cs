using CommandLine;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Util;

namespace YSharp.Cli;

using RunResult = Result<Value, Common.Error>;

internal static class Start
{
    private static void ConsoleRunner()
    {
        RunClass runClass = new();
        Console.WriteLine("Type 'b' anytime to break");

        while (true)
        {
            string inp = Console.ReadLine() ?? string.Empty;

            if (inp == "b")
                break;

            if (inp.Trim() == "")
                continue;

            RunResult res = runClass.Run("<stdin>", inp); // run the app

            if (res.IsFailed)
                Console.WriteLine(res.GetError());
        }
    }

    private static void ScriptRunner(string path)
    {
        RunClass runClass = new();
        // intrnr = internal runner
        RunResult res = runClass.Run(
            "<intrnr>",
            $"RUN(\"{path.Replace("\\", "\\\\")}\")"
        ); // run the app
        if (res.IsFailed)
            Console.WriteLine(res.GetError());
    }

    private static void Main(string[] args)
    {
        CommandLine
            .Parser.Default.ParseArguments<CliArgs>(args)
            .WithParsed(cliargs =>
            {
                RunClass.args = cliargs;
                if (cliargs.ScriptPath is null)
                    ConsoleRunner();
                else
                    ScriptRunner(cliargs.ScriptPath);
            });
    }
}

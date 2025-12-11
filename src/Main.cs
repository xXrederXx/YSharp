using CommandLine;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Utils;
using Error = YSharp.Types.Common.Error;

namespace YSharp;

internal static class Start{
    private static void ConsoleRunner()
    {
        RunClass runClass = new();
        Console.WriteLine("Type 'b' anytime to break");

        while (true)
        {
            string inp = Console.ReadLine() ?? string.Empty;

            if (inp == "b") break;

            if (inp.Trim() == "") continue;

            (Value, Error) res = runClass.Run("<stdin>", inp); // run the app

            if (res.Item2.IsError) Console.WriteLine(res.Item2);
        }
    }

    private static void ScriptRunner(string path)
    {
        RunClass runClass = new();
        // intrnr = internal runner
        (Value, Error) res = runClass.Run("<intrnr>",$"RUN(\"{path.Replace("\\", "\\\\")}\")"); // run the app
        if (res.Item2.IsError) Console.WriteLine(res.Item2);
    }

    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CliArgs>(args)
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
using CommandLine;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Utils;
using Error = YSharp.Types.Common.Error;

namespace YSharp;

internal static class Start
{
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

    private static void Main(string[] args)
    {
        Action func = ConsoleRunner;
        Parser.Default.ParseArguments<CliArgs>(args)
            .WithParsed(cliargs =>
            {
                RunClass.DoExportAstDot = cliargs.RenderDot;
                RunClass.OptimizationLevel = cliargs.Optimization;
                func.Invoke();
            });
    }
}
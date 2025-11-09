using CommandLine;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Utils;

namespace YSharp;
internal class Start
{
    private static void Main(string[] args)
    {
        Action func = ConsoleRunner;
        CommandLine
            .Parser.Default.ParseArguments<CliArgs>(args)
            .WithParsed(cliargs =>
            {
                RunClass.DoExportAstDot = cliargs.RenderDot;
                RunClass.OptimizationLevel = cliargs.Optimization;
                func.Invoke();
            });
    }

    private static void ConsoleRunner()
    {
        /* Console.WriteLine("Type 'e' now to enable log");
        bool logTextEnabled = Console.ReadLine() == "e"; */

        RunClass runClass = new();
        Console.WriteLine("Type 'b' anytime to break");

        while (true)
        {
            string inp = Console.ReadLine() ?? string.Empty;

            if (inp == "b")
            {
                break;
            }

            if (inp.Trim() == "")
            {
                continue;
            }

            (Value, Types.Common.Error) res = runClass.Run("<stdin>", inp); // run the app

            if (res.Item2.IsError)
            {
                Console.WriteLine(res.Item2);
            }
            /* if (logTextEnabled)
            {
                Console.WriteLine(res.Item1 + "\n" + res.Item2); // log the results
            } */
        }
    }
}

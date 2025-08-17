using System.Diagnostics;
using CommandLine;
using YSharp.Benchmarks;
using YSharp.Core;
using YSharp.Types.AST;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Types.Lexer;
using YSharp.Utils;

namespace YSharp;

public class CliArgs
{
    [Option(
        'b',
        "run-bench",
        Required = false,
        HelpText = "This flag is used to run the benchmarks"
    )]
    public bool RunBench { get; set; }

    [Option('t', "run-test", Required = false, HelpText = "This flag is used to run the tests")]
    public bool RunTest { get; set; }
}

internal class Start
{
    private static void Main(string[] args)
    {
        Action func = ConsoleRunner;
        CommandLine
            .Parser.Default.ParseArguments<CliArgs>(args)
            .WithParsed(cliargs =>
            {
                if (cliargs.RunTest)
                    func = RunTest;
                if (cliargs.RunBench)
                    func = TestRunner;
                func.Invoke();
            });
    }

    private static void RunTest()
    {
        (Value, Types.Common.Error) res = new RunClass().Run("<stdin>", "RUN(\"tests/every.ys\")");

        if (res.Item2.IsError)
        {
            Console.WriteLine(res.Item2);
        }
    }

    private static void TestRunner()
    {
        BenchHelp.Run<LexerBench>();
        BenchHelp.Run<ParserBench>();
        BenchHelp.Run<InterpreterBench>();
        BenchHelp.Run<RunTimeBench>();
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

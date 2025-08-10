using System.Diagnostics;
using YSharp.Benchmarks;
using YSharp.Core;
using YSharp.Types.AST;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Types.Interpreter.ClassTypes;
using YSharp.Types.Interpreter.FunctionTypes;
using YSharp.Types.Lexer;
using YSharp.Utils;

namespace YSharp;

// The main entrance Point
internal class Start
{
    private static void Main(string[] args)
    {
        BenchHelp.Run<LexerBench>();
    }

    private static void RunTest()
    {
        (Value, Error) res = new RunClass().Run("<stdin>", "RUN(\"tests/every.ys\")");

        if (res.Item2.IsError)
        {
            Console.WriteLine(res.Item2);
        }
    }

    private static void TestRunner(string change = "-")
    {
        BenchHelp.Run<LexerBench>(change);
        BenchHelp.Run<ParserBench>(change);
        BenchHelp.Run<InterpreterBench>(change);
        BenchHelp.Run<RunTimeBench>(change);
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

            (Value, Error) res = runClass.Run("<stdin>", inp); // run the app

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
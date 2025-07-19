using System.Diagnostics;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using YSharp.Benchmarks;
using YSharp.Internal;
using YSharp.Types;
using YSharp.Types.ClassTypes;
using YSharp.Types.FunctionTypes;
using YSharp.Types.InternalTypes;

namespace YSharp;

// The main entrance Point
internal class Start
{
    private static void Main(string[] args)
    {
        ConsoleRunner();
    }

    private static void TestRunner()
    {
        BenchmarkRunner.Run<LexerBench>();
        BenchmarkRunner.Run<ParserBench>();
        BenchmarkRunner.Run<InterpreterBench>();
        BenchmarkRunner.Run<RunTimeBench>();
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

internal class RunClass
{
    private readonly SymbolTable globalSymbolTable = new();

    public RunClass()
    {
        // predifined values
        globalSymbolTable.Set("TRUE", new VBool(true));
        globalSymbolTable.Set("FALSE", new VBool(false));

        globalSymbolTable.Set("MATH", new VMath());
        globalSymbolTable.Set("PRINT", BuiltInFunctionsTable.print);
        globalSymbolTable.Set("INPUT", BuiltInFunctionsTable.input);
        globalSymbolTable.Set("RUN", BuiltInFunctionsTable.run);
        globalSymbolTable.Set("TIMETORUN", BuiltInFunctionsTable.timetorun);
        globalSymbolTable.Set("TIME", BuiltInFunctionsTable.time);
    }

    public (Value, Error) Run(string fn, string text)
    {
        // create a Lexer and generate the tokens with it
        Lexer lexer = new(text, fn);
        (List<IToken>, Error) tokens = lexer.MakeTokens();

        // look if the lexer threw an Error
        if (tokens.Item2.IsError)
        {
            return (new VNumber(0), tokens.Item2);
        }

        //* For testing -> foreach (Token tok in tokens.Item1){Console.WriteLine(tok.ToString());}

        // create a Parser and parse all the tokens
        Parser parser = new(tokens.Item1);
        ParseResult ast = parser.Parse(); // ast = abstract syntax tree

        //* For Testing ->Console.WriteLine(ast.ToString());

        if (ast.HasError)
        {
            return (new VNumber(0), ast.Error);
        }

        Context context = new("<program>", null, new()) { symbolTable = globalSymbolTable };

        RunTimeResult result = Interpreter.Visit(ast.Node, context);

        // return the node and Error
        return (result.value, result.error);
    }

    public (Value, Error, List<long>) RunTimed(string fn, string text)
    {
        List<long> times = [];
        Stopwatch sw = new();
        Stopwatch WholeTime = new();
        WholeTime.Start();
        // 1: init lexer
        sw.Start();
        Lexer lexer = new(text, fn);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 2: create tokens
        sw.Restart();
        (List<IToken>, Error) tokens = lexer.MakeTokens();

        if (tokens.Item2.IsError)
        {
            return (new VNumber(0), tokens.Item2, times);
        }
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 3: init parser
        sw.Restart();
        Parser parser = new(tokens.Item1);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 4: create ast
        sw.Restart();
        ParseResult ast = parser.Parse(); // ast = abstract syntax tree

        if (ast.Error.IsError)
        {
            return (new VNumber(0), ast.Error, times);
        }
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 6: init context
        sw.Restart();
        Context context = new("<program>", null, new()) { symbolTable = globalSymbolTable };
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 7: run interpreter
        sw.Restart();
        RunTimeResult result = Interpreter.Visit(ast.Node, context);
        sw.Stop();
        times.Add(sw.ElapsedTicks);
        WholeTime.Stop();
        times.Add(WholeTime.ElapsedMilliseconds);
        // return the node and Error
        return (result.value, result.error, times);
    }
}

using System.Diagnostics;

namespace YSharp_2._0;


// The main entrance Point
internal class Start
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Type 'e' now to enable log");
        bool logTextEnabled = Console.ReadLine() == "e";

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

            if(res.Item2.IsError){
                Console.WriteLine(res.Item2);
            }
            if (logTextEnabled)
            {
                Console.WriteLine(res.Item1 + "\n" + res.Item2); // log the results
            }
        }
    }
}

internal  class RunClass
{
    private readonly SymbolTable globalSymbolTable = new();
    public RunClass()
    {
        // predifined values
        globalSymbolTable.Set("TRUE", new Bool(true));
        globalSymbolTable.Set("FALSE", new Bool(false));

        globalSymbolTable.Set("MATH", new Math());
        globalSymbolTable.Set("PRINT", BuiltInFunctionsTable.print);
        globalSymbolTable.Set("INPUT", BuiltInFunctionsTable.input);
        globalSymbolTable.Set("RUN", BuiltInFunctionsTable.run);
        globalSymbolTable.Set("TIMETORUN", BuiltInFunctionsTable.timetorun);
        globalSymbolTable.Set("TIME", BuiltInFunctionsTable.time);

    }
    public  (Value, Error) Run(string fn, string text)
    {
        // create a Lexer and generate the tokens with it
        Lexer lexer = new(text, fn);
        (List<Token>, Error) tokens = lexer.MakeTokens();

        // look if the lexer threw an Error
        if (tokens.Item2.IsError)
        {
            return (new Number(0), tokens.Item2);
        }

        //* For testing -> foreach (Token tok in tokens.Item1){Console.WriteLine(tok.ToString());}

        // create a Parser and parse all the tokens
        Parser parser = new(tokens.Item1);
        ParseResult ast = parser.Parse(); // ast = abstract syntax tree

        //* For Testing ->Console.WriteLine(ast.ToString());

        if (ast.HasError)
        {
            return (new Number(0), ast.Error);
        }

        // create Interpreter for iterpreting code
        Interpreter interpreter = new();

        Context context = new("<program>", null, new())
        {
            symbolTable = globalSymbolTable
        };

        RunTimeResult result = interpreter.Visit(ast.Node, context);

        // return the node and Error
        return (result.value, result.error);
    }

    public (Value, Error, List<long>) RunTimed(string fn, string text)
    {
        List<long> times = [];
        Stopwatch sw = new();

        // 1: init lexer
        sw.Start();
        Lexer lexer = new(text, fn);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 2: create tokens
        sw.Restart();
        (List<Token>, Error) tokens = lexer.MakeTokens();

        if (tokens.Item2.IsError)
        {
            return (new Number(0), tokens.Item2, times);
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
            return (new Number(0), ast.Error, times);
        }
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 5: init interpreter
        sw.Restart();
        Interpreter interpreter = new();
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 6: init context
        sw.Restart();
        Context context = new("<program>", null, new())
        {
            symbolTable = globalSymbolTable
        };
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 7: run interpreter
        sw.Restart();
        RunTimeResult result = interpreter.Visit(ast.Node, context);
        sw.Stop();
        times.Add(sw.ElapsedTicks);
    
        // return the node and Error
        return (result.value, result.error, times);
    }
}


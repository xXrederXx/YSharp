using System.Diagnostics;
using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser;
using YSharp.Runtime;
using YSharp.Runtime.Functions;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Primatives.Number;
using YSharp.Runtime.Utils.Math;
using YSharp.Tools.Debug.Dot;

namespace YSharp.Util;

public class RunClass
{
    public static CliArgs args = new CliArgs() {Optimization = 0, RenderDot = false, ScriptPath = null};
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
        (List<IToken> tokens, Error LexerError) = new Lexer.Lexer(text, fn).MakeTokens();

        // look if the lexer threw an Error
        if (LexerError.IsError) return (new VNumber(0), LexerError);

        // create a Parser and parse all the tokens
        ParseResult ast = new Parser.Parser(tokens).Parse();
        if (args.Optimization > 0) ast = new ParseResult().Success(Optimizer.Optimizer.Visit(ast.Node));
        if (args.RenderDot)
        {
            if (!Directory.Exists("./DOT"))
                Directory.CreateDirectory("./DOT");
            AstDotExporter.ExportToDot(
                ast.Node,
                $"./DOT/DOT-{string.Concat(fn.Where(c => char.IsLetterOrDigit(c)))}.dot"
            );
        }

        if (ast.HasError) return (new VNumber(0), ast.Error);

        Context context = new("<program>", null, new Position()) { symbolTable = globalSymbolTable };
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
        Lexer.Lexer lexer = new(text, fn);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 2: create tokens
        sw.Restart();
        (List<IToken>, Error) tokens = lexer.MakeTokens();

        if (tokens.Item2.IsError) return (new VNumber(0), tokens.Item2, times);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 3: init parser
        sw.Restart();
        Parser.Parser parser = new(tokens.Item1);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 4: create ast
        sw.Restart();
        ParseResult ast = parser.Parse(); // ast = abstract syntax tree

        if (ast.Error.IsError) return (new VNumber(0), ast.Error, times);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 6: init context
        sw.Restart();
        Context context = new("<program>", null, new Position()) { symbolTable = globalSymbolTable };
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
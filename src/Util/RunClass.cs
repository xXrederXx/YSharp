using System.Diagnostics;
using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser;
using YSharp.Runtime;
using YSharp.Runtime.Primatives.Number;
using YSharp.Tools.Debug.Dot;

namespace YSharp.Util;

using LexerResult = Result<List<BaseToken>, Error>;
using RunResult = Result<Value, Error>;

public class RunClass
{
    private readonly SymbolTable globalSymbolTable;

    public RunClass()
    {
        // predifined values
        globalSymbolTable = SymbolTable.GenerateGlobalSymboltable();
    }

    public RunResult Run(string fn, string text)
    {
        // create a Lexer and generate the tokens with it
        LexerResult lexerResult = new Lexer.Lexer(text, fn).MakeTokens();

        // look if the lexer threw an Error
        if (!lexerResult.TryGetValue(out List<BaseToken> tokens))
            return RunResult.Fail(lexerResult.GetError());

        // create a Parser and parse all the tokens
        ParseResult ast = new Parser.Parser(tokens).Parse();
        ast = TryOptimize(ast);
        TryRenderDot(fn, ast);

        if (ast.HasError)
            return RunResult.Fail(ast.Error);

        Context context = new("<program>", null, Position.Null)
        {
            symbolTable = globalSymbolTable,
        };
        RunTimeResult result = Interpreter.Visit(ast.Node, context);

        // return the node and Error
        if (result.error.IsError)
            return RunResult.Fail(result.error);
        return RunResult.Succses(result.value);
    }

    private static void TryRenderDot(string fn, ParseResult ast)
    {
        if (!ArgsHolder.UserArgs.RenderDot)
        {
            return;
        }

        if (!Directory.Exists("./DOT"))
            Directory.CreateDirectory("./DOT");
        AstDotExporter.ExportToDot(
            ast.Node,
            $"./DOT/DOT-{string.Concat(fn.Where(c => char.IsLetterOrDigit(c)))}.dot"
        );
    }

    private static ParseResult TryOptimize(ParseResult ast)
    {
        if (ArgsHolder.UserArgs.Optimization > 0)
            ast = new ParseResult().Success(Optimizer.Optimizer.Visit(ast.Node));
        return ast;
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
        LexerResult lexerResult = lexer.MakeTokens();

        if (!lexerResult.TryGetValue(out List<BaseToken> tokens))
            return (new VNumber(0), lexerResult.GetError(), times);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 3: init parser
        sw.Restart();
        Parser.Parser parser = new(tokens);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 4: create ast
        sw.Restart();
        ParseResult ast = parser.Parse(); // ast = abstract syntax tree

        if (ast.Error.IsError)
            return (new VNumber(0), ast.Error, times);
        sw.Stop();
        times.Add(sw.ElapsedTicks);

        // 6: init context
        sw.Restart();
        Context context = new("<program>", null, new Position())
        {
            symbolTable = globalSymbolTable,
        };
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

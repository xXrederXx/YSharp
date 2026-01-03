using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser;
using YSharp.Runtime;
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
}

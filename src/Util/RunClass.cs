using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser;
using YSharp.Parser.Nodes;
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

    public RunResult Run(string fn, string text, CliArgs args)
    {
        // create a Lexer and generate the tokens with it
        LexerResult lexerResult = new Lexer.Lexer(text, fn).MakeTokens();

        // look if the lexer threw an Error
        if (!lexerResult.TryGetValue(out List<BaseToken> tokens))
            return RunResult.Fail(lexerResult.GetError());

        // create a Parser and parse all the tokens
        ParseResult ast = new Parser.Parser(tokens).Parse();
        if (ast.HasError)
            return RunResult.Fail(ast.Error);

        BaseNode rootNode = args.Optimization > 0 ? Optimizer.Optimizer.Visit(ast.Node) : ast.Node;

        if (args.RenderDot)
            RenderDot(fn, rootNode);

        Context context = new("<program>", null, Position.Null)
        {
            symbolTable = globalSymbolTable,
        };
        RunTimeResult result = Interpreter.Visit(rootNode, context);

        // return the node and Error
        if (result.error.IsError)
            return RunResult.Fail(result.error);
        return RunResult.Succses(result.value);
    }

    private static void RenderDot(string fn, BaseNode node)
    {
        if (!Directory.Exists("./DOT"))
            Directory.CreateDirectory("./DOT");
        AstDotExporter.ExportToDot(
            node,
            $"./DOT/DOT-{string.Concat(fn.Where(c => char.IsLetterOrDigit(c)))}.dot"
        );
    }
}

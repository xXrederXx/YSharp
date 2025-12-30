using BenchmarkDotNet.Attributes;
using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Benchmarks;


public class LexerBench
{
    public bool x;

    [Benchmark]
    public void LexerBenchmarkL()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text50000Char, "BENCHMARK");
        (List<BaseToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkM()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text10000Char, "BENCHMARK");
        (List<BaseToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkS()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text5000Char, "BENCHMARK");
        (List<BaseToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }

    [Benchmark]
    public void LexerBenchmarkXL()
    {
        Lexer.Lexer lexer = new(BenchHelp.Text100000Char, "BENCHMARK");
        (List<BaseToken>, Error) tokens = lexer.MakeTokens();
        x = tokens.Item2.IsError;
    }
}
